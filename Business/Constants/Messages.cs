using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Core.Entities.Concrete;
using Entities.Concrete;

namespace Business.Constants
{
    public static class Messages
    {
        public static string ProductAdded = "Ürün eklendi";
        public static string ProductNameInvalid = "Ürün ismi geçersiz";
        public static string MaintenanceTime = "Sistem bakımda";
        public static string ProductListed = "Ürün listelendi";
        public static string ProductCountOfCategoryError="Bir kategoride en fazla 10 ürün olabilir";
        public static string ProductNameAlreadyExists="Böyle bir ürün ismi var";
        public static string ManyCategories;
        public static string AuthorizationDenied="Yetkiniz yok.";
        public static string UserRegistered;
        public static string UserNotFound="Kullanıcı Bulunamadı";
        public static string PasswordError="Şifre hatalı.";
        public static string SuccessfulLogin;
        internal static string UserAlreadyExists="Bu kullanıcı zaten giriş yapmıştı";
        internal static string AccessTokenCreated;
    }
}
