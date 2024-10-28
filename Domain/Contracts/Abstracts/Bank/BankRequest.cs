using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts.Abstracts.Bank
{
    public class BankRequest
    {
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
        public string BANK_ID { get; set; }
        public string ACCOUNT_NO { get; set; }
        public string TEMPLATE { get; set; }
        public double AMOUNT { get; set; }
        public string DESCRIPTION { get; set; }
        public string ACCOUNT_NAME { get; set; }
    }
}
