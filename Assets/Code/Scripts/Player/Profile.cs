
namespace Global.Framework
{

public class Profile
{
    private static Profile instance = null;

    static public Profile GetProfile()
    {
        if (Profile.instance == null)
        {
            instance = new Profile();
        }

        return instance;
    }
}

}
