using UnityEngine;

using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[System.Serializable]

    public struct _Item_Pos
    {
        public int x;
        public int y;
    }
    public class Inventory
    {
            //아이템넣기 최대수제한에 따라 넣기
            //아이템빼기

            public int[,] Inv_kind;
            public int[,] Inv_num;
            public int Width;
            public int Height;
            public const int DrawWidth = 50;
            public const int DrawHeight = 50;

            public const int MPN = 1;// MaxPossesNum // 최대소지개수


            public Inventory(int width, int height)
            {
                Width = width;
                Height = height;
                Inv_kind = new int[Height, Width];
                Inv_num = new int[Height, Width];
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        Inv_kind[i, j] = -1;
                    }
                }

                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        Inv_num[i, j] = -1;
                    }
                }

            }

            public _Item_Pos Search_Item(int Item_kind)
            {
                _Item_Pos Item_Pos;
                Item_Pos.x = -1;
                Item_Pos.y = -1;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (Inv_kind[i, j] == Item_kind)
                        {
                            if (Inv_num[i, j] >= MPN)
                            {
                                continue;
                            }
                            Item_Pos.x = j;
                            Item_Pos.y = i;
                            return Item_Pos;
                        }
                    }
                }
                return Item_Pos;
            }
            public _Item_Pos Search_Item_Sub(int Item_kind)
            {
                _Item_Pos Item_Pos;
                Item_Pos.x = -1;
                Item_Pos.y = -1;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (Inv_kind[i, j] == Item_kind)
                        {
                            if (Inv_num[i, j] >= MPN)
                            {
                                Item_Pos.x = -j;
                                Item_Pos.y = -i;
                                return Item_Pos;
                            }
                            Item_Pos.x = j;
                            Item_Pos.y = i;
                            return Item_Pos;
                        }
                    }
                }
                return Item_Pos;
            }
            public int Sum_Item(int Item_kind)
            {
                int Sum = 0;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (Inv_kind[i, j] == Item_kind)
                        {
                            Sum += Inv_num[i, j];
                        }
                    }
                }
                return Sum;
            }
            public _Item_Pos Search_Space()
            {
                _Item_Pos Item_Pos;
                Item_Pos.x = -1;
                Item_Pos.y = -1;
                for (int i = 0; i < Height; i++)
                {
                    for (int j = 0; j < Width; j++)
                    {
                        if (Inv_kind[i, j] == -1)
                        {
                            Item_Pos.x = j;
                            Item_Pos.y = i;
                            return Item_Pos;
                        }
                    }
                }
                return Item_Pos;
            }

            public bool Input_Item(int Item_kind, int num)
            {

                bool Inputed = false;
                int Temp = 0;
                for (int i = 0; i < Height && Inputed == false; i++)
                {
                    for (int j = 0; j < Width && Inputed == false; j++)
                    {

                        if ((Search_Item(Item_kind).x != -1 && Search_Item(Item_kind).y != -1))//아이템이 이미 있을때
                        {
                            if ((Inv_num[Search_Item(Item_kind).y, Search_Item(Item_kind).x] + num) > MPN)
                            {
                                Temp = Inv_num[Search_Item(Item_kind).y, Search_Item(Item_kind).x];
                                Inv_num[Search_Item(Item_kind).y, Search_Item(Item_kind).x] += MPN - Inv_num[Search_Item(Item_kind).y, Search_Item(Item_kind).x];
                                num -= MPN - Temp;
                            }
                            else
                            {
                                Inv_num[Search_Item(Item_kind).y, Search_Item(Item_kind).x] += num;
                                num = 0;
                            }
                            Inputed = true;
                            //Inv_kind
                        }
                        else
                        {
                            if (Search_Space().x == -1)
                                return false;

                            int Space_x = Search_Space().x;
                            int Space_y = Search_Space().y;

                            Inv_kind[Space_y, Space_x] = Item_kind;
                            if ((Inv_num[Space_y, Space_x] + num) > MPN)
                            {
                                Inv_num[Space_y, Space_x] = MPN;
                                num -= MPN;
                            }
                            else
                            {
                                Inv_num[Space_y, Space_x] = num;
                                num = 0;
                            }
                            Inputed = true;
                        }
                    }
                }

                if (num > 0)
                {
                    Input_Item(Item_kind, num);
                }
                return Inputed;
            }
            public bool Subtract_Item(int Item_kind, int num)
            {
                if (num > Sum_Item(Item_kind))
                    return false;

                bool Subtracted = false;
                int Temp = 0;
                for (int i = 0; i < Height && Subtracted == false; i++)
                {
                    for (int j = 0; j < Width && Subtracted == false; j++)
                    {

                        if (Search_Item_Sub(Item_kind).x != -1 && Search_Item_Sub(Item_kind).y != -1)//아이템이 이미 있을때
                        {
                            if ((Inv_num[Search_Item_Sub(Item_kind).y, Search_Item_Sub(Item_kind).x] - num) < 0)
                            {
                                Temp = Inv_num[Search_Item_Sub(Item_kind).y, Search_Item_Sub(Item_kind).x];
                                Inv_num[Search_Item_Sub(Item_kind).y, Search_Item_Sub(Item_kind).x] = -1;
                                int Search_x = Search_Item_Sub(Item_kind).x;
                                int Search_y = Search_Item_Sub(Item_kind).y;
                                num -= Temp;
                                Inv_kind[Search_y, Search_x] = -1;
                            }
                            else
                            {
                                Inv_num[Search_Item_Sub(Item_kind).y, Search_Item_Sub(Item_kind).x] -= num;
                                num = 0;
                                if (Inv_num[Search_Item_Sub(Item_kind).y, Search_Item_Sub(Item_kind).x] == 0)
                                {
                                    Inv_num[Search_Item_Sub(Item_kind).y, Search_Item_Sub(Item_kind).x] = -1;
                                    int Search_x = Search_Item_Sub(Item_kind).x;
                                    int Search_y = Search_Item_Sub(Item_kind).y;
                                    Inv_kind[Search_y, Search_x] = -1;

                                }
                            }
                        }
                    }
                }

                if (num > 0)
                {
                    Subtract_Item(Item_kind, num);
                }
                return true;
            }


        
    }