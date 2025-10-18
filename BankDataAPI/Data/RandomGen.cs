using BusinessLayerAPI.Models.Request;
using System.CodeDom;

namespace BusinessLayerAPI.Data
{

    //I just reused the create request objects because they possesed all the same parameters as the account / users did
    public class RandomGen
    {
        static int id = 10000;
        private static readonly Random rnd = Random.Shared;

        public static readonly string[] FirstNames = new string[]
    {
        "Liam", "Olivia", "Noah", "Emma", "Oliver", "Ava", "William", "Sophia", "Ethan", "Isabella",
        "James", "Mia", "Alexander", "Charlotte", "Daniel", "Aurora", "Matthew", "Evelyn", "Aiden", "Harper",
        "Joseph", "Abigail", "Samuel", "Scarlett", "David", "Amelia", "Elijah", "Charlotte", "James", "Abigail",
        "Benjamin", "Emily", "Lucas", "Elizabeth", "Henry", "Mila", "Alexander", "Chloe", "Michael", "Luna",
        "Jacob", "Eleanor", "Logan", "Penelope", "Jackson", "Layla", "Maverick", "Zoey", "Carter", "Riley",
        "Caleb", "Evelyn", "Isaac", "Harper", "Gabriel", "Sofia", "Nathan", "Avery", "Leo", "Ella",
        "David", "Scarlett", "Joseph", "Victoria", "Samuel", "Madison", "John", "Grace", "Luke", "Natalie",
        "Ryan", "Leah", "Christopher", "Lillian", "Andrew", "Hazel", "Theodore", "Alice", "Finn", "Clara",
        "Julian", "Ruby", "Silas", "Stella", "Arthur", "Maeve", "Louis", "Elena", "George", "Audrey",
        "Owen", "Vivian", "Max", "Iris", "Victor", "Freya", "Harrison", "Rose", "Diego", "Jasmine"
    };

        public static readonly string[] LastNames = new string[]
        {
        "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
        "Hernandez", "Lopez", "Gonzalez", "Wilson", "Perez", "Taylor", "Thomas", "Jackson", "White", "Harris",
        "Martin", "Thompson", "Allen", "Baker", "Nelson", "Moore", "Taylor", "Anderson", "Thomas", "Jackson",
        "White", "Harris", "Martin", "Thompson", "Garcia", "Martinez", "Robinson", "Clark", "Lewis", "Lee",
        "Walker", "Hall", "Allen", "Young", "King", "Wright", "Scott", "Green", "Adams", "Baker",
        "Kim", "Chen", "Wang", "Liu", "Zhang", "Li", "Tanaka", "Sato", "Suzuki", "Yamamoto",
        "Silva", "Costa", "Pereira", "Lima", "Müller", "Schneider", "Weber", "Hofmann", "Dubois", "Bernard",
        "Moreau", "Petit", "Ruiz", "Sanchez", "Torres", "Rodriguez", "Flores", "Nguyen", "Schmidt", "Wagner",
        "Dubois", "Rousseau", "Rossi", "Ferrari", "Oliveira", "Santos", "Al-Mansoori", "Hassan", "Koch", "Fischer",
        "Patel", "Khan", "Singh", "Gupta", "O'Connell", "Murphy", "Doherty", "Gauthier", "Leblanc", "Zimmermann"
        };

        public static readonly string[] Addresses = new string[]
    {
        "7345 Oak St, Springfield IL 62704",
        "12 Elmwood Ave, Portland OR 97205",
        "901 Pine Ridge Rd, Austin TX 78701",
        "330 High St, Manchester M13 9PL",
        "555 Cedar Ln, Miami FL 33101",
        "187 Willow Pkwy, Seattle WA 98101",
        "402 Maple Blvd, Denver CO 80203",
        "610 Broadway, New York NY 10007",
        "299 Sunset Dr, Los Angeles CA 90012",
        "88 River Ct, Boston MA 02108",
        "1024 Market St, Philadelphia PA 19107",
        "14 Main St, Chicago IL 60601",
        "500 Industrial Rd, Dallas TX 75201",
        "77 King St W, Toronto ON M5V 1J3",
        "21 Ocean View Ave, San Diego CA 92101",
        "190 Lake Shore Dr, Milwaukee WI 53202",
        "345 Park Ave, Atlanta GA 30303",
        "11 Downing St, London SW1A 2AA",
        "999 Mountain Rd, Phoenix AZ 85004",
        "44 Quai de Grenelle, Paris 75015",
        "600 University Ave, Palo Alto CA 94301",
        "23 Garden Walk, Houston TX 77002",
        "51 North Rd, Vancouver BC V6B 1L6",
        "800 Gold St, Albuquerque NM 87102",
        "17 Palace St, Sydney NSW 2000",
        "35 Forest Ave, Detroit MI 48226",
        "123 Ahornweg, Berlin 10117",
        "707 Central Pkwy, Kansas City MO 64105",
        "950 Commerce Dr, Tampa FL 33602",
        "45 Rue de la Loi, Brussels 1000",
        "111 Harbor Blvd, Long Beach CA 90802",
        "25 Elm St, Dublin D02",
        "888 Capitol Mall, Sacramento CA 95814",
        "300 Pine St, St. Louis MO 63101",
        "1500 E 5th St, Charlotte NC 28202",
        "678 Beacon Hill, Washington DC 20002",
        "490 Bay St, San Francisco CA 94133",
        "222 Baker St, London NW1 6XE",
        "1600 Amphitheatre Pkwy, Mountain View CA 94043",
        "55 Wall St, New York NY 10005",
        "789 Green St, Philadelphia PA 19147",
        "33 Oakwood Pl, Nashville TN 37203",
        "10 Downing St, London SW1A 2AB",
        "404 Error Rd, Silicon Valley CA 95000",
        "505 Fifth Ave, New York NY 10017",
        "1800 Vine St, Hollywood CA 90028",
        "911 Emergency Ln, Gotham NY 10001",
        "654 Birchwood Rd, Anchorage AK 99501",
        "200 Queen St, Brisbane QLD 4000",
        "101 Front St, San Jose CA 95110",
        "321 Main St, Honolulu HI 96813",
        "777 Casino Blvd, Las Vegas NV 89109",
        "444 River Road, Minneapolis MN 55401",
        "58 Bridge St, Brooklyn NY 11201",
        "199 Water St, Jacksonville FL 32202",
        "60 Rue de Rivoli, Paris 75001",
        "808 Tech Park, Redmond WA 98052",
        "13 Dead End, Salem MA 01970",
        "2468 West Ave, Cleveland OH 44113",
        "90 Yellow Brick Rd, Oz KS 67056",
        "333 South Blvd, Indianapolis IN 46204",
        "1700 Elm St, Cincinnati OH 45202",
        "555 Satellite Dr, Houston TX 77058",
        "1234 North Pole, Rovaniemi 96930",
        "70 East St, Raleigh NC 27601",
        "440 Bluebird Ln, Omaha NE 68102",
        "810 Speedway, Indianapolis IN 46222",
        "2020 Vision Dr, Orlando FL 32801",
        "606 South St, New Orleans LA 70130",
        "1100 Campus Dr, Chapel Hill NC 27514",
        "999 E Street, Lincoln NE 68508",
        "30 West 42nd St, New York NY 10036",
        "555 Market St, San Francisco CA 94105",
        "77 Sunset Strip, West Hollywood CA 90069",
        "1500 Pennsylvania Ave, Washington DC 20500",
        "200 Trade St, Irvine CA 92618",
        "400 Central Ave, Albuquerque NM 87102",
        "888 Gateway Blvd, South San Francisco CA 94080",
        "333 Commerce St, Fort Worth TX 76102",
        "123 Sesame St, Brooklyn NY 11201",
        "600 Peachtree St, Atlanta GA 30308",
        "456 Ocean Dr, Miami Beach FL 33139",
        "1000 Main St, Vancouver WA 98660",
        "700 Industrial Way, San Carlos CA 94070",
        "21 Jump St, New Orleans LA 70112",
        "500 Boylston St, Boston MA 02116",
        "900 Wilshire Blvd, Los Angeles CA 90017",
        "111 First St, Cambridge MA 02142",
        "4040 Park Ave, Chicago IL 60657",
        "800 High Street, Columbus OH 43215",
        "3000 North Blvd, Baltimore MD 21201",
        "1776 Independence Ave, Philadelphia PA 19106",
        "500 Broadway, Nashville TN 37203",
        "2500 West End Ave, Dallas TX 75205",
        "666 Devil's Gate, Hell MI 48169",
        "10 Downing Street, London SW1A 2AA",
        "777 Lucky Lane, Reno NV 89501",
        "4000 Main St, Houston TX 77027",
        "1200 River Rd, Sacramento CA 95831",
        "999 Tech Way, Austin TX 78759",
        "300 South St, Salt Lake City UT 84101",
        "1500 Sunset Blvd, Hollywood CA 90027",
        "5000 Freeway, San Antonio TX 78205"
    };

      
        public static int GenNextAcctNumber()
        {
            id++;
            return id;
        }
        public static int GenRandomBalance()
        {
            return rnd.Next(10, 10000);
        }
        public static String GenRandomFirstName()
        {
            String name = FirstNames[GenIndex()];
            return name;
        }

        public static String GenRandomLastName()
        {
            String name = LastNames[GenIndex()];
            return name;
        }

        public static String GenRandomAddress()
        {
            String address = Addresses[GenIndex()];
            return address;
        }

        public static string GenRandomPhoneNum()
        {
            const long MinValue = 1_000_000_000L;
            const long MaxValue = 9_999_999_999L;

            Random random = Random.Shared;

            long range = MaxValue - MinValue;


            byte[] buf = new byte[8];
            random.NextBytes(buf);

            long longRand = BitConverter.ToInt64(buf, 0) & long.MaxValue;
            long finalNumber = MinValue + (long)(range * (longRand / (double)long.MaxValue));

            return finalNumber.ToString();
        }

        public static int GenIndex()
        {

            Random random = Random.Shared;

            int randomNumber = random.Next(0, 101);

            return randomNumber;
        }


        public static CreateUserRequest GenRandomUser()
        {
            CreateUserRequest user = new CreateUserRequest(
                GenRandomFirstName() + "Handle", //handle
                GenRandomFirstName(), //firstname
                GenRandomLastName(), //lastname
                GenRandomFirstName() + "@RealMail.com", //email
                GenRandomLastName() + "123", //password
                GenRandomAddress(), //address
                GenRandomPhoneNum(), //phone
                null, //photo
                false //isadmin
                );

            return user;
        }
    }
}
