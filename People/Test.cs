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
            user.id = 1;
            user.ListGrade = new List<Grade> {
                new Grade{Item=1 , Score=4.0 },
                new Grade{Item=2 , Score=3.0 },
                new Grade{Item=3 , Score=2.5 },
            };
            List_User.Add(user);
            user = new User();
            user.id = 2;
            user.ListGrade = new List<Grade> {
                new Grade{Item=1 , Score=2.0 },
                new Grade{Item=2 , Score=2.5 },
                new Grade{Item=3 , Score=4.0 },
            };
            List_User.Add(user);
            user = new User();
            user.id = 3;
            user.ListGrade = new List<Grade> {
                new Grade{Item=1 , Score=1.5 },
            };
            List_User.Add(user);
            user = new User();
            user.id = 4;
            user.ListGrade = new List<Grade> {
                new Grade{Item=1 , Score=3.0 },
                new Grade{Item=3 , Score=3.0 },
            };
            List_User.Add(user);
            user = new User();
            user.id = 5;
            user.ListGrade = new List<Grade> {
                new Grade{Item=1 , Score=2.0 },
                new Grade{Item=2 , Score=3.0 },
                new Grade{Item=3 , Score=2.0 },
            };
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

            Hashtable ht = Goal_User.GetFriendlyUser(List_User);
            foreach(DictionaryEntry de in ht) 
                Console.WriteLine("User"+de.Key + "," + "相似度" + de.Value);
            Console.ReadKey();
        }
    }
}
