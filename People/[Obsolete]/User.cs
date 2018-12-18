
//using System;
//using System.Collections;
//using System.Collections.Generic;

//namespace Recommend
//{
//    [Obsolete]
//    class User
//    {
//        public int id { set; get; }
//        public Hashtable Items { set; get; }
//        public Hashtable GetRecommendItem(List<User> users)
//        {
//            Hashtable RecommendItem = new Hashtable();
//            Hashtable RecommendItemNum = new Hashtable();
//            foreach (User user in users)
//            {
//                double Item_Grade = 0;
//                double Degree = GetSimilarityDegree(user);
//                Hashtable NotCommonItem = GetDifferent(user, false);
//                foreach (DictionaryEntry Item in NotCommonItem)
//                {
//                    if (Degree == -1)
//                    {
//                        continue;
//                    }
//                    else
//                    {
//                        if (RecommendItem.ContainsKey(Item.Key))
//                        {
//                            double Item1 = double.Parse(RecommendItem[Item.Key].ToString());
//                            Item_Grade = double.Parse(RecommendItem[Item.Key].ToString())
//                                + double.Parse(user.Items[Item.Key].ToString()) * Degree;
//                            RecommendItem[Item.Key] = Item_Grade;
//                            RecommendItemNum[Item.Key] = int.Parse(RecommendItemNum[Item.Key].ToString()) + 1;
//                        }
//                        else
//                        {
//                            Item_Grade = double.Parse(user.Items[Item.Key].ToString()) * Degree;
//                            RecommendItem.Add(Item.Key, Item_Grade);
//                            RecommendItemNum.Add(Item.Key, 1);
//                        }
//                    }
//                }
//            }
//            //foreach (DictionaryEntry Item in RecommendItemNum) {
//            //    RecommendItem[Item.Key] = double.Parse(RecommendItem[Item.Key].ToString()) 
//            //                            / double.Parse(RecommendItemNum[Item.Key].ToString());
//            //}平均
//            return RecommendItem;
//        }
//        public Hashtable GetSimilarityDegrees(List<User> users)
//        {
//            Hashtable AllSimilarityDegree = new Hashtable();
//            foreach (User user in users) AllSimilarityDegree.Add(user.id, GetSimilarityDegree(user));
//            return AllSimilarityDegree;
//        }
//        public List<User> GetSimilarityDegrees(List<User> users, int K)
//        {

//            Hashtable AllSimilarityDegree = GetSimilarityDegrees(users);
//            List<User> listuser = new List<User>();

//            int[] SimilarityDegreesSort = new int[AllSimilarityDegree.Count];//存储用户相似度排序
//            double[] SimilarityDegreesSort_V = new double[AllSimilarityDegree.Count];//存储用户相似度排序
//            AllSimilarityDegree.Keys.CopyTo(SimilarityDegreesSort, 0);
//            AllSimilarityDegree.Values.CopyTo(SimilarityDegreesSort_V, 0);
//            Array.Sort(SimilarityDegreesSort_V, SimilarityDegreesSort);
//            Array.Reverse(SimilarityDegreesSort);//排序

//            foreach (int i in SimilarityDegreesSort)
//            {
//                if ((K + 1) <= 0) break;
//                K--;
//                //Console.WriteLine("User" + i + ",   " + AllSimilarityDegree[i]);
//                foreach (User u in users)
//                {
//                    if (u.id == i)
//                    {
//                        listuser.Add(u);
//                    }
//                }
//            }

//            return listuser;
//        }
//        private double GetSimilarityDegree(User user)
//        {
//            Hashtable CommonItem = GetDifferent(user, true);
//            double EM = 0;
//            if (CommonItem.Count > 0)
//            {
//                foreach (DictionaryEntry Item in CommonItem)
//                    EM += Math.Pow(double.Parse(Items[Item.Key].ToString())
//                    - double.Parse(user.Items[Item.Key].ToString()), 2);//欧几里德距离
//                return 1 / (1 + Math.Sqrt(EM));//两用户相似度
//            }
//            else return -1;//两用户没一样的
//        }
//        private Hashtable GetDifferent(User user, bool common)//找不同，true表示找相同的，false表示找不同的
//        {
//            Hashtable CommonItem = new Hashtable();
//            foreach (DictionaryEntry user_item in user.Items)
//                if (!this.Items.ContainsKey(user_item.Key) ^ common)
//                    CommonItem.Add(user_item.Key, null);
//            return CommonItem;
//        }
//    }
//}