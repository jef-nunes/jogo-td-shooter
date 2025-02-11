public static class GlobalVariables
{
    public static int level1MinionsKill;
    public static bool boss1Killed;

    public static int GetMinionsKil(int level)
    {
        switch (level)
        {
            case 1:
                return level1MinionsKill;
            default:
            return 0;
        }
    }
    public static void IncMinionsKill(int level)
    {
        switch (level)
        {
            case 0:
            break;
            case 1:
                level1MinionsKill+=1;
                break;
            default:
            break;
        }
    }
}
