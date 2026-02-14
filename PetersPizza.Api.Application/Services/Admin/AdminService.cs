using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetersPizza.Api.Application.Interfaces.Services;
using PetersPizza.Api.Application.SignalR;
using PetersPizza.Api.Infrastructure.Interfaces.Repositories;
using PetersPizza.Api.Models.Admin;
using PetersPizza.Api.Models.SignalR;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace PetersPizza.Api.Application.Services.Admin;

public class AdminService(
    IWebHostEnvironment environment,
    IAdminRepository adminRepository,
    IConfiguration configuration,
    PasswordHasher<LoginAdminRequest> passwordHasher,
    JwtSecurityTokenHandler jwtTokenHandler,
    IHubContext<UserOrdersHub> hubContext) : IAdminService
{
    public async Task UploadPizzaAsync(UploadPizzaRequest request)
    {
        var uploadsFolder = Path.Combine(environment.WebRootPath, "images");
        var fileName = Guid.NewGuid();
        
        Directory.CreateDirectory(uploadsFolder);
        
        var filePath = Path.Combine(uploadsFolder, fileName + Path.GetExtension(request.PizzaName));

        var insertPizzaRequest = new InsertPizzaRequest
        {
            PizzaName = request.PizzaName,
            Description = request.Description,
            PizzaPrice = request.PizzaPrice,
            PizzaImageId = fileName
        };
        
        var copyTask = CompressAndSaveAsync(request.PizzaImage, filePath);
        var persistTask = adminRepository.InsertPizzaAsync(insertPizzaRequest);

        await Task.WhenAll(copyTask, persistTask);
    }

    public async Task<LoginAdminResponse> LoginAdminAsync(LoginAdminRequest request)
    {
        var repositoryResponse = await adminRepository.LoginAdminAsync(request);

        if (!IsValidPassword(request.Password, repositoryResponse.PasswordHash)) return new LoginAdminResponse();

        return new LoginAdminResponse
        {
            Name = repositoryResponse.Name,
            JwtToken = GenerateJwtTokenForAdmin(request.UserName, repositoryResponse.AdminId)
        };
    }

    public async Task<GetAllOrdersResponse> GetAllOrdersAsync()
    {
        var repositoryResponse = await adminRepository.GetAllOrdersForTodayAsync();
        
        var orders = repositoryResponse
            .GroupBy(x => x.OrderIdGuid)
            .Select(g =>
            {
                var first = g.First();

                return new GetAllOrderResponse
                {
                    OrderId = g.Key,
                    UserName = first.UserName,
                    OrderState = first.OrderState,
                    OrderDate = first.OrderDate,
                    OrderItems = g.Select(x => new OrderItem
                    {
                        OrderId = x.OrderIdInteger,
                        PizzaName = x.PizzaName,
                        Quantity = x.Quantity,
                        Price = x.Price
                    })
                };
            })
            .OrderBy(x => x.OrderState)
            .ThenBy(x => x.OrderDate)
            .ToList();

        return new GetAllOrdersResponse
        {
            GetAllOrderResponses = orders
        };
    }

    public async Task ChangeOrderStateAsync(ChangeOrderStateRequest request)
    {
        var userId = await adminRepository.ChangeOrderStateAsync(request);

        if (userId > 0)
        {
            await hubContext.Clients.User(userId.ToString()).SendAsync("ReceiveOrderStatus", new OrderStatusChangedNotification
            {
                OrderId = request.OrderId,
                NewOrderState = request.NewOrderState
            });
        }
    }

    public JwtTokenInformationResponse ExtractJwtInformationFromToken(JwtTokenInformationRequest request)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(request.JwtToken);

        var isAdmin = jwt.Claims.Any(c =>
            string.Equals(c.Value, Constants.AdminRole, StringComparison.OrdinalIgnoreCase));
        
        return new JwtTokenInformationResponse { IsAdmin = isAdmin };
    }

    private static async Task CompressAndSaveAsync(IFormFile imageFile, string outputPath, int quality = 50)
    {
        await using var inputStream = imageFile.OpenReadStream();
        using var image = await Image.LoadAsync(inputStream);

        image.Mutate(i => i.Resize(new ResizeOptions
        {
            Mode = ResizeMode.Max,
            Size = new Size(300, 300)
        }));
        
        var encoder = new JpegEncoder { Quality = quality };

        await image.SaveAsync(outputPath, encoder);
    }
    
    private bool IsValidPassword(string providedPassword, string storedPasswordHash)
    {
        if (string.IsNullOrWhiteSpace(storedPasswordHash)) return false;
        
        var result = passwordHasher.VerifyHashedPassword(null, storedPasswordHash, providedPassword);
        
        return result == PasswordVerificationResult.Success;
    }
    
    private string GenerateJwtTokenForAdmin(string userName, int userId)
    {
        var key = configuration.GetSection(Constants.JwtKey).Value;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity([
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, Constants.AdminRole),
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString())
            ]),
            Issuer = Constants.ApplicationName,
            Expires = DateTime.UtcNow.AddHours(8),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!)),
                SecurityAlgorithms.HmacSha256Signature)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);
        var responseToken = jwtTokenHandler.WriteToken(token);
        
        return responseToken;
    }
}