using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Recommend
{
    class Test
    {
        public static List<User> GetAllUser()
        {
            List<User> List_User = new List<User>();
            User user = new User();
            Hashtable item = new Hashtable();
            item.Add(1, 2);
            item.Add(2, 0.5);
            item.Add(4, 1);
            user.id = 1;
            user.Items =item;
            List_User.Add(user);

            user = new User();
            item = new Hashtable();
            item.Add(1, 4);
            item.Add(2, 2);
            item.Add(3, 1);
            item.Add(4, 2);
            item.Add(5, 0);
            user.id = 2;
            user.Items = item;
            List_User.Add(user);

            user = new User();
            item = new Hashtable();
            item.Add(1, 1);
            item.Add(3, 2);
            item.Add(4, 1);
            item.Add(5, 0);
            item.Add(6, 1);
            user.id = 3;
            user.Items = item;
            List_User.Add(user);

            user = new User();
            item = new Hashtable();
            item.Add(1, 2);
            item.Add(4, 2);
            item.Add(5, 3);
            user.id = 4;
            user.Items = item;
            List_User.Add(user);

            user = new User();
            item = new Hashtable();
            item.Add(3, 2);
            item.Add(4, 2);
            item.Add(5, 0);
            item.Add(6, 1);
            user.id = 5;
            user.Items = item;
            List_User.Add(user);

            return List_User;
        }
        static void Main(string[] args)
        {
            List<User> List_User = GetAllUser();
            User Goal_User = new User { id = 1 };//目标用户id为1

            foreach (User user in List_User)
                if (Goal_User.id == user.id)
                    Goal_User = user;

            Hashtable ht = Goal_User.GetSimilarityDegrees(List_User);
            foreach(DictionaryEntry de in ht) 
                Console.WriteLine("User"+de.Key + ",   " + de.Value);
            Console.WriteLine("Enter Get Recomment");
            Console.ReadKey();

            Hashtable ht1 = Goal_User.GetRecommendItem(List_User);
            foreach (DictionaryEntry de in ht1)
                Console.WriteLine("Item" + de.Key + ",   " + de.Value);
            Console.ReadKey();


        }
    }
}
