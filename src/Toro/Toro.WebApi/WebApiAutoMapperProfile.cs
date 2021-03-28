using AutoMapper;
using Toro.Domain.Models;
using Toro.WebApi.Dtos;

namespace Toro.WebApi
{
    public class WebApiAutoMapperProfile : Profile
    {
        public WebApiAutoMapperProfile()
        {
            CreateMap<TransactionEventOriginAccount, Account>();
            CreateMap<TransactionEventTargetAccount, Account>()
                .ForMember(d => d.AccountNumber,
                            opt => opt.MapFrom(s => s.Account));
            CreateMap<TransactionEventPost, TransactionEvent>();
        }
    }
}
