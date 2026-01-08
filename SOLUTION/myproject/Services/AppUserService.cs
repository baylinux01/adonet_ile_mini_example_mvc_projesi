using System.Text.RegularExpressions;
using myproject.Models;
using myproject.Repositories;

namespace myproject.Services
{
    public class AppUserService
    {
        private AppUserRepository aur;
        public AppUserService(AppUserRepository appUserRepository)
        {
            this.aur=appUserRepository;
        }

        public Result ControlLogin(String username,String password)
        {
            Result result=new Result();
            if(username==null || password==null
            || username.Trim().Equals("")||password.Trim().Equals(""))
            {
                result.Success=false;
                result.Code="00";
                result.Message="Kullanıcı adı veya şifre kriterlere uygun değil";
            }
            else
            {
                bool success=aur.CheckLogin(username,password);
                if(success==true)
                {
                    result.Success=true;
                    result.Code="200";
                    result.Message="Giriş Başarılı";
                }
                else
                {
                    result.Success=false;
                    result.Code="-11";
                    result.Message="Kullanıcı adı veya şifre yanlış";
                }
            }
            return result;
        }

        public Result Add(String name,String surname,String username,String password1, String password2)
        {
            Result result=new Result();
            if(name==null||name.Trim().Equals("")
            ||surname==null||surname.Trim().Equals("")
            ||username==null||username.Trim().Equals("")
            ||password1==null||password1.Trim().Equals("")
            ||password2==null||password2.Trim().Equals("")
            ||!password1.Equals(password2)
            ||!Regex.IsMatch(name,"^[a-zA-Z0-9\\._\\-ÖöÇçİıĞğÜüŞş]+$")
            ||!Regex.IsMatch(surname,"^[a-zA-Z0-9\\._\\-ÖöÇçİıĞğÜüŞş]+$")
            ||!Regex.IsMatch(username,"^[a-zA-Z0-9\\._\\-ÖöÇçİıĞğÜüŞş]+$")
            ||!Regex.IsMatch(password1,"^[a-zA-Z0-9\\._\\-ÖöÇçİıĞğÜüŞş]+$")
            ||!Regex.IsMatch(password2,"^[a-zA-Z0-9\\._\\-ÖöÇçİıĞğÜüŞş]+$"))
            {
                result.Success=false;
                result.Code="00000";
                result.Message="Kayıt formu alanlarından en az biri geçersiz ya da şifreler aynı değil";
            }
            else if(aur.CheckUsernameInUseOrNot(username)==true)
            {
                result.Success=false;
                result.Code="-1";
                result.Message="Bu kullanıcı adı zaten kullanımda";
            }
            else
            {
                AppUser au=new AppUser();
                au.Name=name;
                au.Surname=surname;
                au.Username=username;
                au.Password=password1;
                int affected=aur.Add(au);
                if(affected==0)
                {
                    result.Success=false;
                    result.Code="-2";
                    result.Message="Kullanıcıyı kaydederken bilinmeyen hata";
                }
                else
                {
                    result.Success=true;
                    result.Code="11111";
                    result.Message="Kullanıcı kayıt işlemi başarılı. Giriş Yapabilirsiniz";
                }
            }
            return result;
        }

        public ViewModel GetAppUserByUsername(String username)
        {
            ViewModel vm=new ViewModel();
            Result result=new Result();
            AppUser appUser=null;

            if(username==null 
            || username.Trim().Equals(""))
            {
                result.Success=false;
                result.Code="00";
                result.Message="Kullanıcı adı null veya uygun değil";
            }
            else
            {
                appUser=aur.FindByUsername(username);
                if(appUser!=null)
                {
                    result.Success=true;
                    result.Code="200";
                    result.Message="Kullanıcı username kullanılarak başarıyla bulundu";
                }
                else
                {
                    result.Success=false;
                    result.Code="-1";
                    result.Message="Kullanıcı bulunamadı";
                }
            }
            vm.AppUser=appUser;
            vm.Result=result;
            return vm;
        }
        
    }
}