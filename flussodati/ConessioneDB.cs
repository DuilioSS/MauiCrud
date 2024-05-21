

namespace mauicrud.flussodati
{
    public class ConessioneDB
    {
        public static string ritonastrada(string nomedatabase)
        {
            string stradadatabase = string.Empty;
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                stradadatabase = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                stradadatabase = Path.Combine(stradadatabase, nomedatabase);
            }

            return stradadatabase;

        }
    }
}
