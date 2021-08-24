using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class UserDefine
{
    public const float D_START_X = -160.0f;
    public const float D_START_Y = 100.0f;

    public const float D_BLOCK_WIDTH = 64.0f;
    public const float D_BLOCK_HEIGHT = 64.0f;

    public const int D_MAP_WIDTH = 6;
    public const int D_MAP_HEIGHT = 6;

    public const float D_SIZE = 0.01f;

    static public float ScreenToWorld_X(float fValue)
    {
        return (fValue - 400.0f);
    }

    static public float ScreenToWorld_Y(float fValue)
    {
        return (fValue - 240.0f);
    }
}

