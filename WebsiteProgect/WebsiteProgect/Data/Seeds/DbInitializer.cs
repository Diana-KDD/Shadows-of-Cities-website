using System.Runtime.CompilerServices;
using WebsiteProgect.Models;
using static Azure.Core.HttpHeader;

namespace WebsiteProgect.Data.Seeds
{
    namespace WebsiteProgect.Data.Seeds
    {
        public static class DbInitializer
        {
            public static void Initialize(AppDbContext context)
            {
                //--- Гарантирует, что база создана
                context.Database.EnsureCreated();

                //--- Если уже есть места — выходим, чтобы не задваивать
                if (context.Places.Any()) return;

                //--- Справочники
                var Namibia = new Country { Name = "Намибия" };
                var Japan = new Country { Name = "Япония" };
                var Ukraine = new Country { Name = "Украина" };
                var Italy = new Country { Name = "Италия" };
                var Bulgaria = new Country { Name = "Болгария" };
                var Belgium = new Country { Name = "Бельгия" };
                var GreatBritain = new Country { Name = "Великобритания" };
                var China = new Country { Name = "Китай" };
                var Australia = new Country { Name = "Австралия" };
                context.Countries.AddRange(Namibia, Japan, Ukraine, Italy, 
                    Bulgaria, Belgium, GreatBritain,China, Australia);
                context.SaveChanges();

                var Luderitz = new City { Name = "Людериц", CountryId = Namibia.Id };
                var Nagasaki = new City { Name = "Нагасаки", CountryId = Japan.Id };
                var Pripyat = new City { Name = "Припять", CountryId = Ukraine.Id };
                var Sorrento = new City { Name = "Сорренто", CountryId = Italy.Id };
                var Kazanlak = new City { Name = "Казанлык", CountryId = Bulgaria.Id };
                var Charleroi = new City { Name = "Шарлеруа", CountryId = Belgium.Id };
                var ThamesEstuary = new City { Name = "Устье Темзы", CountryId = GreatBritain.Id };
                var Hangzhou = new City { Name = "Ханчжоу", CountryId = China.Id };
                var Sydney = new City { Name = "Сидней", CountryId = Australia.Id };
                var Nara = new City { Name = "Нара", CountryId = Japan.Id };
                context.Cities.AddRange(Luderitz, Nagasaki, Pripyat, Sorrento, 
                    Kazanlak, Charleroi, ThamesEstuary, Hangzhou, Sydney, Nara);
                context.SaveChanges();

                var GhostTown = new Category { Name = "Город-призрак", IsDefault = false };
                var AbandonedIsland = new Category { Name = "Заброшенный остров", IsDefault = false };
                var Industrialfacility = new Category { Name = "Индустриальный объект", IsDefault = false };
                var IdeologicalStructure = new Category { Name = "Идеологическое сооружение", IsDefault = false };
                var MilitaryFacility = new Category { Name = "Военный объект", IsDefault = false };
                var AbandonedProject = new Category { Name = "Заброшенный проект", IsDefault = false };
                var SunkenShip = new Category { Name = "Затонувшее судно", IsDefault = false };
                var AmusementPark = new Category { Name = "Парк аттракционов", IsDefault = false };

                context.Categories.AddRange(GhostTown, AbandonedIsland, Industrialfacility,
                    IdeologicalStructure, MilitaryFacility, AbandonedProject, SunkenShip,
                    AmusementPark);
                context.SaveChanges();

                //--- Создание мест
                var places = new List<Place>
            {
                new Place
                {
                    Name = "Колманскоп",
                    CategoryId = GhostTown.Id,
                    Category = GhostTown,
                    CityId = Luderitz.Id,
                    City = Luderitz,
                    Description = "Жилые дома, больница и школа, постепенно засыпаемые песками пустыни Намиб. " +
                    "Комнаты старых немецких особняков теперь заполнены сугробами песка.",
                    History = "Основан в 1908 году во время алмазной лихорадки. Здесь были первая в " +
                    "Африке трамвайная линия и рентгеновская станция. Заброшен после того, как " +
                    "запасы алмазов иссякли, а новые месторождения нашли южнее.",
                    YearClosure = "1956 ",
                    CreatedAt = DateTime.UtcNow.AddMonths(3)
                },
                new Place
                {
                    Name = "Хасима",
                    CategoryId = AbandonedIsland.Id,
                    Category = AbandonedIsland,
                    CityId = Nagasaki.Id,
                    City = Nagasaki,
                    Description = "Крошечный остров, сплошь застроенный бетонными жилыми и " +
                    "промышленными зданиями. Из-за характерного силуэта, напоминающего " +
                    "военный корабль, получил прозвище «Боевой остров»",
                    History = "С 1890-х годов здесь добывали уголь. В 1959 году плотность " +
                    "населения была рекордной — более 5000 человек на острове. Закрыт из-за " +
                    "перехода на нефть и нерентабельности добычи угля",
                    YearClosure = "1974",
                    CreatedAt = DateTime.UtcNow.AddMonths(3)
                },
                new Place
                {
                    Name = "Припять",
                    CategoryId = GhostTown.Id,
                    Category = GhostTown,
                    CityId = Pripyat.Id,
                    City = Pripyat,
                    Description = "Советский город атомщиков, обслуживавший Чернобыльскую " +
                    "АЭС. Жилые дома, школы, парк аттракционов и стадион стремительно " +
                    "поглощает природа.",
                    History = "Основана в 1970 году. Полностью эвакуирована 27 апреля 1986 " +
                    "года после аварии на ЧАЭС. Город стал символом техногенной катастрофы и " +
                    "зоной отчуждения.",
                    YearClosure = "1986",
                    CreatedAt = DateTime.UtcNow.AddMonths(3)
                },
                new Place
                {
                    Name = "Долина мельниц",
                    CategoryId = Industrialfacility.Id,
                    Category = Industrialfacility,
                    CityId = Sorrento.Id,
                    City = Sorrento,
                    Description = "Глубокое ущелье, где каменные средневековые " +
                    "мельницы буквально вросли в скалы. Из-за высокой влажности руины " +
                    "покрыты густой тропической зеленью.",
                    History = "Мельницы работали с XIII века, используя силу горных ручьев. " +
                    "Пришли в упадок в XIX веке, когда машинное производство " +
                    "вытеснило ручной труд, и ущелье было заброшено.",
                    YearClosure = "1940",
                    CreatedAt = DateTime.UtcNow.AddMonths(3)
                },
                new Place
                {
                    Name = "Дом-памятник Бузлуджа",
                    CategoryId = IdeologicalStructure.Id,
                    Category = IdeologicalStructure,
                    CityId = Kazanlak.Id,
                    City = Kazanlak,
                    Description = "Футуристическая бетонная «тарелка», венчающая вершину " +
                    "горы. Внутри сохранились мозаики на коммунистическую тематику, " +
                    "хотя здание постепенно разрушается.",
                    History = "Построен в 1981 году как Дом-памятник Болгарской " +
                    "коммунистической партии. После краха социалистического строя в " +
                    "1989 году монумент был разграблен и оставлен.",
                    YearClosure = "1989",
                    CreatedAt = DateTime.UtcNow.AddMonths(3)
                },
                new Place
                {
                    Name = "IM Электростанция",
                    CategoryId = Industrialfacility.Id,
                    Category = Industrialfacility,
                    CityId = Charleroi.Id,
                    City = Charleroi,
                    Description = "Гигантская градирня (башня охлаждения) с огромным " +
                    "круглым залом. Мох на стенах и искаженная акустика создают гнетущую, " +
                    "сюрреалистичную атмосферу.",
                    History = "Построена в 1921 году и долгое время была крупнейшей " +
                    "ТЭЦ Бельгии. Закрыта в 2007 году в том числе из-за протестов " +
                    "Greenpeace, так как давала до 10% выбросов CO₂ в стране.",
                    YearClosure = "2007",
                    CreatedAt = DateTime.UtcNow.AddMonths(3)
                },
                new Place
                {
                    Name = "Морские форты Маунселла",
                    CategoryId = MilitaryFacility.Id,
                    Category = MilitaryFacility,
                    CityId = ThamesEstuary.Id,
                    City = ThamesEstuary,
                    Description = "Группа ржавых металлических платформ на высоких опорах, " +
                    "поднимающихся из воды. Выглядят как декорации к фантастическому фильму.",
                    History = "Построены в 1942 году для защиты Лондона от немецких " +
                    "авианалетов. В 1960-х пустовавшие форты использовали как базы " +
                    "пиратские радиостанции.",
                    YearClosure = "1958",
                    CreatedAt = DateTime.UtcNow.AddMonths(3)
                },
                new Place
                {
                    Name = "Город Тяньдучэн",
                    CategoryId = AbandonedProject.Id,
                    Category = AbandonedProject,
                    CityId = Hangzhou.Id,
                    City = Hangzhou,
                    Description = "«Маленький Париж» — жилой комплекс с копией Эйфелевой " +
                    "башни высотой 108 метров и имитацией французской архитектуры. Широкие " +
                    "бульвары почти безлюдны.",
                    History= "Амбициозный проект девелоперов начала 2000-х, рассчитанный " +
                    "на 10,000 жителей. Стал городом-призраком из-за сильной удаленности и " +
                    "высокой стоимости жилья.",
                    YearClosure = "2010",
                    CreatedAt = DateTime.UtcNow.AddMonths(3)
                },
                new Place
                {
                    Name = "Заброшенный корабль SS Ayrfield",
                    CategoryId = SunkenShip.Id,
                    Category = SunkenShip,
                    CityId = Sydney.Id,
                    City = Sydney,
                    Description = "Ржавый сухогруз длиной 80 метров, стоящий в заливе " +
                    "Хоумбуш. Из его трюмов растут целые мангровые леса, превращая корабль в «Плавучий лес».",
                    History = "Судно спущено на воду в 1911 году, служило для перевозки " +
                    "угля и участвовало в снабжении войск во Второй мировой. В 1972 году " +
                    "выведено из эксплуатации и оставлено на разборку, который не состоялся.",
                    YearClosure = "1972",
                    CreatedAt = DateTime.UtcNow.AddMonths(3)
                },
                new Place
                {
                    Name = "Нара Дримлэнд",
                    CategoryId = AmusementPark.Id,
                    Category = AmusementPark,
                    CityId = Nara.Id,
                    City = Nara,
                    Description = "Аналог Диснейленда с собственным замком и монорельсом. " +
                    "Аттракционы и карусели застыли без движения, постепенно разрушаясь.",
                    History = "Открыт в 1961 году как ответ американским паркам. " +
                    "Закрыт из-за низкой посещаемости после того, как поблизости " +
                    "появились Tokyo Disneyland и Universal Studios Japan.",
                    YearClosure = "2006",
                    CreatedAt = DateTime.UtcNow.AddMonths(3)
                }
            };
                context.Places.AddRange(places);
                context.SaveChanges();

                //--- Тестовые картинки
                var path = "/uploads/";
                List<Image> images = new List<Image>()
                {
                    new Image
                    {
                        ImageName = "Долина мельниц_1.png",
                        ImagePath = path + "Долина мельниц_1.png",
                        PlaceId = places[3].Id,
                        Place = places[3],
                        IsPrimary = true
                    },
                    new Image
                    {
                        ImageName = "Колманскоп_1.png",
                        ImagePath = path + "Колманскоп_1.png",
                        PlaceId = places[0].Id,
                        Place = places[0],
                        IsPrimary = true
                    },
                    new Image
                    {
                        ImageName = "МорскиефортыМаунселла_1.jpg",
                        ImagePath = path + "МорскиефортыМаунселла_1.jpg",
                        PlaceId = places[6].Id,
                        Place = places[6],
                        IsPrimary = true
                    },
                    new Image
                    {
                        ImageName = "МорскиефортыМаунселла_2.png",
                        ImagePath = path + "МорскиефортыМаунселла_2.png",
                        PlaceId = places[6].Id,
                        Place = places[6],
                        IsPrimary = true
                    },
                    new Image
                    {
                        ImageName = "НараДримлэнд_1.png",
                        ImagePath = path + "НараДримлэнд_1.png",
                        PlaceId = places[9].Id,
                        Place = places[9],
                        IsPrimary = true
                    },
                    new Image
                    {
                        ImageName = "Припять_1.jpg",
                        ImagePath = path + "Припять_1.jpg",
                        PlaceId = places[2].Id,
                        Place = places[2],
                        IsPrimary = true
                    },
                    new Image
                    {
                        ImageName = "Хасима_1.png",
                        ImagePath = path + "Хасима_1.png",
                        PlaceId = places[1].Id,
                        Place = places[1],
                        IsPrimary = true
                    },
                    new Image
                    {
                        ImageName = "Хасима_2.png",
                        ImagePath = path + "Хасима_2.png",
                        PlaceId = places[1].Id,
                        Place = places[1],
                        IsPrimary = true
                    },
                    new Image
                    {
                        ImageName = "Хасима_3.jpg",
                        ImagePath = path + "Хасима_3.jpg",
                        PlaceId = places[1].Id,
                        Place = places[1],
                        IsPrimary = true
                    }
                };
                context.Images.AddRange(images);
                context.SaveChanges();
            }
        }
    }
}
