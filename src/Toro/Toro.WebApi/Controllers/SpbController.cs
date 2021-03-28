using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Toro.Domain.Models;
using Toro.Domain.Services;
using Toro.WebApi.Dtos;

namespace Toro.WebApi.Controllers
{
    [Route("spb")]
    [ApiController]
    public class SpbController : ControllerBase
    {
        #region Fields

        private readonly IAccountService accountService;
        private readonly IMapper mapper;

        #endregion

        #region Constructors

        public SpbController(IAccountService accountService)
        {
            this.accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));

            var mapperConfig = new MapperConfiguration(f =>
            {
                f.AddProfile<WebApiAutoMapperProfile>();
            });
            mapper = mapperConfig.CreateMapper();
        }

        #endregion

        #region Public Methods

        [HttpPost("events")]
        public async Task<IActionResult> PostEvent([FromBody]TransactionEventPost transactionEventPost)
        {
            var transactionEvent = mapper.Map<TransactionEvent>(transactionEventPost);

            var result = await accountService.Deposit(transactionEvent);

            return Ok(result);
        }

        #endregion
    }
}
