using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;

namespace IoTSuper_DesktopApp.Helpers
{
    public static class Navegacion
    {
        public static Action<UserControl>? CambiarVista;

        public static void IrA(UserControl vista)
        {
            CambiarVista?.Invoke(vista);
        }
    }
}
