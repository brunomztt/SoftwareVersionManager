using SoftwareVersionManager.Models;

namespace SoftwareVersionManager.Data;

public static class DbInitializer
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Softwares.Any())
            return;

        var softwares = new List<Software>
        {
            new Software
            {
                Name = "Visual Studio Code",
                Description = "Editor de código leve e multiplataforma",
                Developer = "Microsoft",
                Versions = new List<SoftwareVersion>
                {
                    new SoftwareVersion
                    {
                        VersionNumber = "1.99.0",
                        ReleaseNotes = "Melhorias de performance e correções",
                        ReleaseDate = DateTime.UtcNow.AddMonths(-2),
                        IsDeprecated = false
                    },
                    new SoftwareVersion
                    {
                        VersionNumber = "1.85.0",
                        ReleaseNotes = "Versão antiga",
                        ReleaseDate = DateTime.UtcNow.AddMonths(-10),
                        IsDeprecated = true
                    }
                }
            },

            new Software
            {
                Name = "Docker Desktop",
                Description = "Plataforma de containers",
                Developer = "Docker Inc.",
                Versions = new List<SoftwareVersion>
                {
                    new SoftwareVersion
                    {
                        VersionNumber = "4.40.0",
                        ReleaseNotes = "Atualização de segurança",
                        ReleaseDate = DateTime.UtcNow.AddMonths(-1),
                        IsDeprecated = false
                    }
                }
            },

            new Software
            {
                Name = "Postman",
                Description = "Ferramenta para testes de APIs",
                Developer = "Postman Inc.",
                Versions = new List<SoftwareVersion>
                {
                    new SoftwareVersion
                    {
                        VersionNumber = "11.0.0",
                        ReleaseNotes = "Nova interface",
                        ReleaseDate = DateTime.UtcNow.AddMonths(-3),
                        IsDeprecated = false
                    }
                }
            }
        };

        context.Softwares.AddRange(softwares);
        context.SaveChanges();
    }
}