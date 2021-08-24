using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NewMng
{

    static public Vector3 m_New_Vector = new Vector3();
    static public Quaternion m_New_Quaternion = new Quaternion();
    static public Color m_New_Color = new Color();

    static public Vector3 New_Vector3(float x, float y, float z)
    {
        m_New_Vector.Normalize();
        m_New_Vector.x = x;
        m_New_Vector.y = y;
        m_New_Vector.z = z;

        return m_New_Vector;
    }

    static public Color New_Color(float r, float g, float b, float a)
    {
        m_New_Color.r = r;
        m_New_Color.g = g;
        m_New_Color.b = b;
        m_New_Color.a = a;

        return m_New_Color;
    }

    static public Quaternion New_Quaternion(float x, float y, float z, float w)
    {
        m_New_Quaternion.x = 0.0f;
        m_New_Quaternion.y = 0.0f;
        m_New_Quaternion.z = 0.0f;
        m_New_Quaternion.w = 0.0f;

        m_New_Quaternion.x = x;
        m_New_Quaternion.y = y;
        m_New_Quaternion.z = z;
        m_New_Quaternion.w = w;

        return m_New_Quaternion;
    }
}
