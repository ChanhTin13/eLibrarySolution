using System;
using System.Collections.Generic;
using System.Text;

namespace Utilities
{
    public class CoreContants
    {

        public const string DEFAULT_PASSWORD = "12345678";

        public const string UPLOAD_FOLDER_NAME = "Upload";
        public const string TEMP_FOLDER_NAME = "Temp";
        public const string TEMPLATE_FOLDER_NAME = "Template";
        public const string CATALOGUE_TEMPLATE_NAME = "CatalogueTemplate.xlsx";
        public const string USER_FOLDER_NAME = "User";
        public const string QR_CODE_FOLDER_NAME = "QRCode";

        public const string GET_TOTAL_NOTIFICATION = "get-total-notification";
        public enum typeExam
        {
            trac_nghiem = 1,
            tu_luan
        }

        public enum statusExamResult
        {
            chua_cham_bai = 1,
            da_cham_bai
        }

        public enum roleNewsFeedGroup
        {
            quan_tri_vien = 1,
            thanh_vien
        }
         
        public enum feedbackStatus
        {
            moi_gui = 1,
            dang_xu_ly,
            da_xong
        }

        public enum taskStudent
        {           
            dang_lam = 1,
            hoan_thanh,
            chua_lam
        }

        public enum exerciseLevel
        {
            difficult = 1,
            normal,
            easy
        }
        public enum studentInClassStatus
        {
            learning = 1,
            pass,
            fail
        }
        public enum rollUpLearning
        {
            gioi,
            kha,
            trung_binh,
            kem,
            theo_doi_dac_biet,
            co_co_gang,
            khong_co_gang,
            khong_nhan_xet
        }
        public enum rollUpStatus
        {
            co,
            vang_co_phep,
            vang_khong_phep,
            di_muon,
            ve_som,
            nghi_le
        }
        public enum classStatus
        {
            upcoming = 1,
            opening,
            closed
        }
        public enum reviewGroupType
        {
            multiple_choice = 1,
            essay
        }
        public enum pointColumnType
        {
            point = 1,
            average,
            note
        }
        public enum necessaryType
        {
            payment_method = 1,
            sample_config,
            study_time_config
        }
        public enum role
        {
            admin = 1,
            moderator,//Điều phối viên
            teacher,
            student
        }
        public enum gender
        {
            male = 1,
            female,
            other
        }
        public enum userStatus
        {
            active,
            locked
        }
        #region Catalogue Name
        /// <summary>
        /// Phường
        /// </summary>
        public const string WARD_CATALOGUE_NAME = "Ward";

        /// <summary>
        /// Quốc gia
        /// </summary>
        public const string COUNTRY_CATALOGUE_NAME = "Country";

        /// <summary>
        /// Quận
        /// </summary>
        public const string DISTRICT_CATALOGUE_NAME = "District";

        /// <summary>
        /// Thành phố
        /// </summary>
        public const string CITY_CATALOGUE_NAME = "City";

        /// <summary>
        /// Dân tộc
        /// </summary>
        public const string NATION_CATALOGUE_NAME = "Nation";

        /// <summary>
        /// Loại thông báo
        /// </summary>
        public const string NOTIFICATION_TYPE_CATALOGUE_NAME = "NotificationType";
        #endregion

        #region SMS Template
        /// <summary>
        /// Xác nhận OTP SMS
        /// </summary>
        public const string SMS_XNOTP = "XNOTP";
        #endregion

        #region Email Template
        #endregion
    }
}
