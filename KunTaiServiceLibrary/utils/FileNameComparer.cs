using System.Collections;

namespace KunTaiServiceLibrary
{
    public class FileNameComparer : IComparer
    {
        public int Compare(object a, object b)
        {
            string s1 = a.ToString();
            string s2 = b.ToString();
            bool flag = false;
            bool flag1 = false;
            bool flag2 = false;
            int end1 = 0;
            int end2 = 0;
            int start = 0;
            int min = s1.Length;
            if (min > s2.Length)
                min = s2.Length;

            for (int i = 0; i < min; i++)
            {
                if (!flag)
                {
                    end1 = i;
                    end2 = i;
                    start = i;


                    if (s1[i] >= 48 && s1[i] <= 57 && s2[i] >= 48 && s2[i] <= 57)
                    {
                        flag = true;
                        continue;
                    }
                    else if (s1[i] == s2[i])
                        continue;
                    else
                        return s1[i] > s2[i] ? 1 : -1;
                }
                else
                {
                    if (flag1 && flag2)
                    {
                        int num1 = int.Parse(s1.Substring(start, end1 - start + 1));
                        int num2 = int.Parse(s2.Substring(start, end2 - start + 1));
                        if (num1 == num2)
                        {
                            flag = false;
                            flag1 = false;
                            flag2 = false;
                            continue;
                        }
                        else return num1 > num2 ? 1 : -1;
                    }
                    else
                    {
                        if (!flag1)
                        {
                            if (s1[i] < 48 || s1[i] > 57)
                            {
                                end1 = i - 1;
                                flag1 = true;
                            }
                        }

                        if (!flag2)
                        {
                            if (s2[i] < 48 || s2[i] > 57)
                            {
                                end2 = i - 1;
                                flag2 = true;
                            }
                        }
                    }
                }
            }
            if (s1.Length == s2.Length)
                return 0;
            else return s1.Length > s2.Length ? 1 : -1;
        }
    }
}
