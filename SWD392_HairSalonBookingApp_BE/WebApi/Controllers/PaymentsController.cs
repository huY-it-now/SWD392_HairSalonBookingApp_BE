using Application.Interfaces;
using Application.Services;
using Application.Utils;
using AutoMapper;
using AutoMapper.Internal;
using Domain.Contracts.Abstracts.Bank;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Bank;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MimeKit.Utils;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService paymentService, IMapper mapper)
        {
            _paymentService = paymentService;
            _mapper = mapper;
        }


        #region QuickLinkEnity
        /*
            <BANK_ID> có thể là:

            Mã BIN ngân hàng, được quy định bởi ngân hàng nhà nước
            Tên thường gọi phổ biến / tên viết tắt quy ước bởi NAPAS của ngân hàng
            Ví dụ ngân hàng Vietinbank có mã BIN là 970415, và thường được gọi là Vietinbank , Napas quy ước tên viết tắt là ICB thì chúng ta có thể sử dụng BANK_ID là 970415 hoặc Vietinbank hoặc ICB đều được.

            Tra cứu danh sách BIN(bin)/ tên thường gọi (short_name) / tên viết tắt (code) của các ngân hàng bằng API danh sách ngân hàng

            ACCOUNT_NO: 

            Số tài khoản người nhận tại ngân hàng thụ hưởng.
            Số tài khoản người nhận quy ước bao gồm chữ hoặc số, tối đa 19 kí tự.

            TEMPLATE:
            <TEMPLATE> quy định ID của template. Template là hình thức trình bày của file ảnh chứa mã VietQR.

            AMOUNT: quy định số tiền chuyển khoản

            Số tiền chuyển khoản phải là một số dương và tối đa 13 chữ số.

            DESCRIPTION: quy định nội dung chuyển khoản.

            Nội dung chuyển khoản bao gồm tối đa 50 chữ cái, không bao gồm các kí tự đặc biệt.

            ACCOUNT_NAME: quy định tên người thụ hưởng hiển thị lên file ảnh VietQR. Trường này không nằm trong tiêu chuẩn tạo mã VietQR.
         */
        #endregion
        [HttpPost("QuickLink")]
        [ProducesResponseType(200, Type = typeof(Result<object>))]
        [ProducesResponseType(400, Type = typeof(Result<object>))]
        public async Task<IActionResult> QuickLink([FromBody] BankRequest bankRequest)
        {
            string linkImage = $"https://img.vietqr.io/image/{bankRequest.BANK_ID}-{bankRequest.ACCOUNT_NO}-{bankRequest.TEMPLATE}.png?amount={bankRequest.AMOUNT}&addInfo={bankRequest.DESCRIPTION}&accountName={bankRequest.ACCOUNT_NAME}";

            return Ok(linkImage);
        }
    }
}
