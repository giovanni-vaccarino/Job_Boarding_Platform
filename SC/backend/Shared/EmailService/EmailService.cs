using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

namespace backend.Shared.EmailService;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly IAmazonSimpleEmailService _sesClient;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;

        _sesClient = new AmazonSimpleEmailServiceClient(
            _configuration["AWS:AccessKey"],
            _configuration["AWS:SecretKey"],
            RegionEndpoint.GetBySystemName(_configuration["AWS:EmailRegion"])
        );
    }

    public async Task SendEmailAsync(string toAddress, string subject, string body)
    {
        var sendRequest = new SendEmailRequest
        {
            Source = _configuration["SES:Email"], 
            Destination = new Destination
            {
                ToAddresses = new List<string> { toAddress }
            },
            Message = new Message
            {
                Subject = new Content(subject),
                Body = new Body
                {
                    Html = new Content(body)
                }
            }
        };

        try
        {
            var response = await _sesClient.SendEmailAsync(sendRequest);
            Console.WriteLine($"Email sent. Message ID: {response.MessageId}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Email failed to send: {ex.Message}");
            throw;
        }
    }
}
