using Application.Interfaces;
using Application.Services;
using Application.Utils;
using AutoMapper;
using AutoMapper.Internal;
using Azure;
using Domain.Contracts.Abstracts.Bank;
using Domain.Contracts.Abstracts.Shared;
using Domain.Contracts.DTO.Bank;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MimeKit.Utils;
using Net.payOS;
using Net.payOS.Types;
using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.Crmf;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IBookingService _bookingService;
        private readonly PayOS _payOS;
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;

        public PaymentsController(IPaymentService paymentService, IMapper mapper, PayOS payOS, IBookingService bookingService)
        {
            _bookingService = bookingService;
            _payOS = payOS;
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
        public async Task<IActionResult> QuickLink([FromBody] BankRequest bankRequest, Guid BookingId)
        {
            var paymentCheck = await _paymentService.BookingPaymentCheck(BookingId);

            if (!paymentCheck)
            {
                return BadRequest("The booking is not exist!");
            }

            if (await _paymentService.ChangePaymentStatus(BookingId, "Pending"))
            {
                string linkImage = $"https://img.vietqr.io/image/{bankRequest.BANK_ID}-{bankRequest.ACCOUNT_NO}-{bankRequest.TEMPLATE}.png?amount={bankRequest.AMOUNT}&addInfo={bankRequest.DESCRIPTION}&accountName={bankRequest.ACCOUNT_NAME}";

                return Ok(linkImage);
            }
            else
            {
                return Ok("This booking is paid");
            }
        }

        [HttpPost("create")]
        public async Task<Result<object>> CreatePaymentLink(Guid bookingId)
        {
            var result = new Result<object>
            {
                Error = 0,
                Message = "",
                Data = null
            };

            try
            {
                var booking = await _bookingService.GetBookingById(bookingId);
                int orderCode = int.Parse(DateTimeOffset.Now.ToString("ffffff"));
                ItemData item = new ItemData(booking.ComboService.ComboServiceName, 1, decimal.ToInt32(booking.ComboService.Price));
                var cancelUrl = "https://localhost:7152/api/Payments/CancelPayment";
                var returnUrl = "https://localhost:7152/swagger/index.html";
                List<ItemData> items = new List<ItemData>();
                items.Add(item);
                PaymentData paymentData = new PaymentData(orderCode, decimal.ToInt32(booking.ComboService.Price), "Thanh toan doan hang", items, cancelUrl, returnUrl);

                CreatePaymentResult createPayment = await _payOS.createPaymentLink(paymentData);

                result.Message = "success";
                result.Data = createPayment;
                return result;
            }
            catch
            {
                result.Message = "Create fail";
                result.Error = -1;
                return result;
            }
        }

        [HttpPut("CancelPayment")]
        public async Task<Result<object>> CancelPayment([FromRoute] int orderId)
        {
            var result = new Result<object>
            {
                Error = 0,
                Message = "",
                Data = null
            };

            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.cancelPaymentLink(orderId);

                result.Message = "Cancel success";
                result.Data = paymentLinkInformation;
                return result;
            }
            catch
            {
                result.Message = "Cancel fail";
                result.Error = -1;
                return result;
            }
        }

        [HttpGet("GetPayment")]
        public async Task<Result<object>> GetPayment([FromRoute] int orderId)
        {
            var result = new Result<object>
            {
                Error = 0,
                Message = "",
                Data = null
            };

            try
            {
                PaymentLinkInformation paymentLinkInformation = await _payOS.getPaymentLinkInformation(orderId);

                result.Message = "Get success";
                result.Data = paymentLinkInformation;
                return result;
            }
            catch
            {
                result.Message = "Get fail";
                result.Error = -1;
                return result;
            }
        }

        [HttpPost("confirm-webhook")]
        public async Task<Result<object>> ConfirmWebhook(string webhook_url )
        {
            var result = new Result<object>
            {
                Error = 0,
                Message = "",
                Data = null
            };

            try
            {
                await _payOS.confirmWebhook(webhook_url);
                result.Message = "success";
                return result;
            }
            catch
            {
                result.Message = "fail";
                result.Error = -1;
                return result;
            }

        }

        [HttpPost("payos_transfer_handler")]
        //Webhook của cửa hàng dùng để nhận dữ liệu thanh toán từ payOS,
        public Result<object> payOSTransferHandler(WebhookType body)
        {
            var result = new Result<object>
            {
                Error = 0,
                Message = "",
                Data = null
            };

            try
            {
                WebhookData data = _payOS.verifyPaymentWebhookData(body);

                if (data.description == "Ma giao dich thu nghiem" || data.description == "VQRIO123")
                {
                    result.Message = "Get success";
                    return result;
                }
                result.Message = "Get success";
                return result;
            }
            catch
            {
                result.Message = "Get fail";
                result.Error = -1;
                return result;
            }

        }
    }
}
