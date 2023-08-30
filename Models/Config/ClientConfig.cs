using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Sms.Models.Config
{
    public class ClientConfig : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasData(
                new Client()
                {
                    Id = 1,
                    Name = "Yahya",
                    Description="deneme Açıklaması",
                    PhoneNumber = "5418145813"
                }

                );
        }
    }
}
