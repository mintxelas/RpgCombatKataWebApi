using HashidsNet;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using RpgCombat.Application.CreateRpgCharacter;
using RpgCombat.Application.DamageRpgCharacter;
using RpgCombat.Application.GetRpgCharacter;
using RpgCombat.Application.HealRpgCharacter;
using System.Linq;

namespace RpgCombat.WebApi.Tests
{
    public class CustomWebApplicationFactory<T>: WebApplicationFactory<T> where T: class 
    {
        public IHashids Hashids { get; set; }
        public IRequestHandler<CreateRpgCharacterRequest, CreateRpgCharacterResponse> CreateRpgCharacterHandler { get; set; }
        public IRequestHandler<DamageRpgCharacterRequest, DamageRpgCharacterResponse> DamageRpgCharacterHandler { get; set; }
        public IRequestHandler<HealRpgCharacterRequest, HealRpgCharacterResponse> HealRpgCharacterHandler { get; set; }
        public IRequestHandler<GetRpgCharacterRequest, GetRpgCharacterResponse> GetRpgCharacterHandler { get; set; }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var hashidsDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(IHashids));
                services.Remove(hashidsDescriptor);
                services.AddScoped(_ => Hashids);

                var createRpgCharacterHandlerDescriptor = services.SingleOrDefault(s =>
                    s.ServiceType == typeof(IRequestHandler<CreateRpgCharacterRequest, CreateRpgCharacterResponse>));
                services.Remove(createRpgCharacterHandlerDescriptor);
                services.AddScoped(_ => CreateRpgCharacterHandler);

                var damageRpgCharacterHandlerDescriptor = services.SingleOrDefault(s =>
                    s.ServiceType == typeof(IRequestHandler<DamageRpgCharacterRequest, DamageRpgCharacterResponse>));
                services.Remove(damageRpgCharacterHandlerDescriptor);
                services.AddScoped(_ => DamageRpgCharacterHandler);

                var healRpgCharacterHandlerDescriptor = services.SingleOrDefault(s =>
                    s.ServiceType == typeof(IRequestHandler<HealRpgCharacterRequest, HealRpgCharacterResponse>));
                services.Remove(healRpgCharacterHandlerDescriptor);
                services.AddScoped(_ => HealRpgCharacterHandler);

                var getRpgCharacterHandlerDescriptor = services.SingleOrDefault(s =>
                    s.ServiceType == typeof(IRequestHandler<GetRpgCharacterRequest, GetRpgCharacterResponse>));
                services.Remove(getRpgCharacterHandlerDescriptor);
                services.AddScoped(_ => GetRpgCharacterHandler);
            });
        }
    }
}