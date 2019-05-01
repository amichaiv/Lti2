using System;
using System.Collections.Generic;

namespace StudentsAccessor
{
    public class Person
    {
        public Person(string name, string email)
        {
            Name = name;
            Email = email;
        }

        public int Id
        {
            get
            {
                Random random = new Random();
                return random.Next(111111, 999999);
            }
        }
        public string Name { get; set; }
        public string Email { get; set; }

        public char Group
        {
            get
            {
                Random random = new Random();
                int num = random.Next(0, 4); // Zero to 25
                return (char)('A' + num);
            }
        }

        public DateTime SubmissionTime => DateTime.Now;

        public string Status
        {
            get
            {
                Array values = Enum.GetValues(typeof(Status));
                return ((Status)values.GetValue(new Random().Next(values.Length))).ToString();
            }
        }

        public int Grade
        {
            get
            {
                Random random = new Random();
                return random.Next(0, 100);
            }
        }

        public int ProgressValue
        {
            get
            {
                Random random = new Random();
                return random.Next(0, 100);
            }
        }

        public bool IsCheated
        {
            get
            {
                Random random = new Random();
                return random.Next(2) == 0;
            }
        }

        public int Budget => 150;

        public int Consumed
        {
            get
            {
                Random random = new Random();
                return random.Next(0, 150);
            }
        }


        public static IEnumerable<Person> GetPersons()
        {
            var persons = new List<Person>
            {
                new Person("Itay Yaffe","itayy@codevalue.net"),
                new Person("Amichai Vaknin", "amichai@codevalue.net"),
                new Person("Alex Pshul","alex@codevalue.net"),
                new Person("Yarin Mansour","yarin@codevalue.net"),
                new Person("Suzan Zaher","suzan@codevalue.net"),
                new Person("Moti Hadash","moti@codevalue.net"),
                new Person("Amir Segal","amir@codevalue.net"),
                new Person("Benny Reznik","benny@codevalue.net"),
                new Person("Idan Malka","idan@codevalue.net"),
                new Person("Aviv Shalom","aviv@codevalue.net"),
                new Person("Shoola Mokshim","shoola@codevalue.net"),
                new Person("Ran Yaakov","ran@codevalue.net"),
                new Person("Dror Lavi","dror@codevalue.net"),
                new Person("Alon Fliss","alon@codevalue.net"),
                new Person("Libi Hadad","libi@codevalue.net"),
                new Person("Ben Hornado","ben@codevalue.net"),
                new Person("Danny Kaplunsky","danny@codevalue.net"),
                new Person("We Sum","we@codevalue.net")
            };

            return persons;
        }
    }
}

