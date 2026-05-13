using IoTSuper_DesktopApp.Modelos;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace IoTSuper_DesktopApp.Config
{
    public static class Sesion
    {
        public static ApiConfigFolder ApiConfigFolder = new ApiConfigFolder();
        public static LoginResponse LoginData = new LoginResponse();
        public static readonly string msiName = Assembly.GetExecutingAssembly().GetName().Name;
    }
}
