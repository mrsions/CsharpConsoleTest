using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleTest
{
    public class JavascriptEnum
    {
        public static async void Run()
        {
            Print<ResultStatus>();
        }
        static void Print<T>()
        { 
            var arr = (T[])Enum.GetValues(typeof(T));
            Array.Sort(arr);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"const {typeof(T).Name} = {{");
            foreach(var a in arr)
            {
                sb.AppendLine($"    {a}: {a.GetHashCode()},");
            }
            sb.AppendLine("};");
            Console.WriteLine(sb);
        }


        public enum ResultStatus
        {
            InternalClientError = -3,
            InternalServerError = -2,
            Unknown = -1,

            // Common
            Ok = 0,
            No,
            Failed,
            Cancel,
            Expires,
            NotAuth,
            NotFound,
            BadRequest,
            Conflict,
            MissMatch,
            Blocked,
            TimeOut,
            NoContent,
            NoGrant,
            Pending,
            TryAgain,
            Already,
            Longer,
            Short,

            // Sign
            SIGN = 10_000,
            SIGN_NeedTwoFactor,
            SIGN_EmailNeedConfirm,
            SIGN_EmailAlready,
            SIGN_PhoneNumberNeedConfirm,
            SIGN_PhoneNumberAlready,
            SIGN_DisplayNameNeedConfirm,
            SIGN_DisplayNameAlready,

            // Account
            ACCOUNT = 100_000,
            ACCOUNT_EmailProblems,
            ACCOUNT_EmailProblems_Invalid,
            ACCOUNT_EmailProblems_Short,
            ACCOUNT_EmailProblems_Long,
            ACCOUNT_PasswordProblems,
            ACCOUNT_PasswordProblems_Invalid,
            ACCOUNT_PasswordProblems_Short,
            ACCOUNT_PasswordProblems_Long,
            ACCOUNT_CurrentPasswordProblems,
            ACCOUNT_CurrentPasswordProblems_Invalid,
            ACCOUNT_CurrentPasswordProblems_Short,
            ACCOUNT_CurrentPasswordProblems_Long,
            ACCOUNT_NewPasswordProblems,
            ACCOUNT_NewPasswordProblems_Invalid,
            ACCOUNT_NewPasswordProblems_Short,
            ACCOUNT_NewPasswordProblems_Long,
            ACCOUNT_DisplayNameProblems,
            ACCOUNT_DisplayNameProblems_Invalid,
            ACCOUNT_DisplayNameProblems_Short,
            ACCOUNT_DisplayNameProblems_Long,
            ACCOUNT_PhoneNumberProblems,
            ACCOUNT_PhoneNumberProblems_Wrong,
            ACCOUNT_AddressProblems,
            ACCOUNT_AddressProblems_Invalid,
            ACCOUNT_AddressProblems_Short,
            ACCOUNT_AddressProblems_Long,
            ACCOUNT_GenderProblems,
            ACCOUNT_GenderProblems_Invalid,
            ACCOUNT_PhotoProblems,
            ACCOUNT_PhotoProblems_Invalid,
            ACCOUNT_PhotoProblems_OverSize,
            ACCOUNT_PhotoProblems_OverBounds,

            // Document
            DOCUMENT = 200_000,
            DOCUMENT_SubjectProblems,
            DOCUMENT_SubjectProblems_Invalid,
            DOCUMENT_SubjectProblems_Short,
            DOCUMENT_SubjectProblems_Long,
            DOCUMENT_ContentsProblems,
            DOCUMENT_ContentsProblems_Invalid,
            DOCUMENT_ContentsProblems_Short,
            DOCUMENT_ContentsProblems_Long,

            // Category
            CATEGORY = 210_000,
            CATEGORY_Problems_NotFound,
            CATEGORY_IdProblems,
            CATEGORY_IdProblems_Invalid,
            CATEGORY_IdProblems_Short,
            CATEGORY_IdProblems_Long,
            CATEGORY_Policy_Delete,
            CATEGORY_Policy_Edit,
            CATEGORY_Policy_Write,
            CATEGORY_Policy_UploadFile,
            CATEGORY_Policy_Edit_Commented,
            CATEGORY_Policy_Delete_Commented,
            CATEGORY_Policy_Write_Comment,
            CATEGORY_Policy_Read,

            // File Upload
            FILE_UPLOAD_Problems = 220_000,
            FILE_UPLOAD_NotFound,
            FILE_UPLOAD_OutOfSize,
            FILE_UPLOAD_NotSupport_Extension,
        }
    }
}
