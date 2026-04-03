using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Project_OOP_2._0.Cat;

namespace Project_OOP_2._0
{
    internal class GameEngine
    {
        //OOP Concept applied: ENCAPSULATION (private fields, and public properties)
        // 1. Private Fields 
        private House house;
        private MainCharacter mainCharacterCat;
        private Character husband;
        private Character wife;
        private Character mistress;
        private Cat greyCat;
        private Cat whiteCat;
        private List<HouseSpace> houseSpaceList;
        private IntroScene intro;
        private Scene1 scene1;
        private Scene2 scene2;
        private Scene3 scene3;
        private Scene4 scene4;
        private Scene5 scene5;
        private Scene6 scene6;

        // 2. Public Properties 
        public House House
        {
            get { return house; }
            set { house = value; }
        }
        public MainCharacter MainCharacterCat
        {
            get { return mainCharacterCat; }
            set { mainCharacterCat = value; }
        }
        public Character Husband
        {
            get { return husband; }
            set { husband = value; }
        }
        public Character Wife
        {
            get { return wife; }
            set { wife = value; }
        }
        public Character Mistress
        {
            get { return mistress; }
            set { mistress = value; }
        }
        public Cat GreyCat
        {
            get { return greyCat; }
            set { greyCat = value; }
        }
        public Cat WhiteCat
        {
            get { return whiteCat; }
            set { whiteCat = value; }
        }
        public List<HouseSpace> HouseSpaceList
        {
            get { return houseSpaceList; }
            set { houseSpaceList = value; }
        }
        public IntroScene Intro
        {
            get { return intro; }
            set { intro = value; }
        }
        public Scene1 Scene1
        {
            get { return scene1; }
            set { scene1 = value; }
        }
        public Scene2 Scene2
        {
            get { return scene2; }
            set { scene2 = value; }
        }
        public Scene3 Scene3
        {
            get { return scene3; }
            set { scene3 = value; }
        }
        public Scene4 Scene4
        {
            get { return scene4; }
            set { scene4 = value; }
        }
        public Scene5 Scene5
        {
            get { return scene5; }
            set { scene5 = value; }
        }
        public Scene6 Scene6
        {
            get { return scene6; }
            set { scene6 = value; }
        }

        // 3. Constructor
        public GameEngine()
        {
            this.House = new House();
            this.MainCharacterCat = new MainCharacter("\x1b[38;2;255;153;51m");
            this.Husband = new Character("\x1b[38;2;47;19;209m");
            this.Wife = new Character("\x1b[38;2;230;55;224m");
            this.Mistress = new Character("\x1b[38;2;235;23;45m");
            this.GreyCat = new Cat("\x1b[38;2;92;90;90m");
            this.WhiteCat = new Cat("\x1b[38;2;235;232;232m");
            this.HouseSpaceList = new List<HouseSpace>();

            this.SetupHouseItems();

            this.Intro = new IntroScene("Intro");
            this.Scene1 = new Scene1("Scene 1");
            this.Scene2 = new Scene2("Scene 2");
            this.Scene3 = new Scene3("Scene 3");
            this.Scene4 = new Scene4("Scene 4");
            this.Scene5 = new Scene5("Scene 5");
            this.Scene6 = new Scene6("Scene 6");
        }

        // 4. Methods
        public void SetupHouseItems()
        {
            //Bathroom 1
            HouseSpace Bathroom1 = new HouseSpace("Bathroom 1");
            PrimaryItem Bathroom1ToiletBowl = new PrimaryItem("Toilet Bowl", Bathroom1.Name);
            PrimaryItem Bathroom1Pail = new PrimaryItem("Pail", Bathroom1.Name);
            PrimaryItem Bathroom1Tap = new PrimaryItem("Tap", Bathroom1.Name);
            PrimaryItem Bathroom1Hose = new PrimaryItem("Hose", Bathroom1.Name);
            PrimaryItem Bathroom1Shower = new PrimaryItem("Shower", Bathroom1.Name);
            PrimaryItem Bathroom1Soap = new PrimaryItem("Soap", Bathroom1.Name);
            List<PrimaryItem> itemsInBath1 = new List<PrimaryItem> { Bathroom1ToiletBowl, Bathroom1Pail, Bathroom1Tap, Bathroom1Hose, Bathroom1Shower, Bathroom1Soap };
            Bathroom1.ItemsAvailable.AddRange(itemsInBath1);

            //Bedroom
            HouseSpace Bedroom = new HouseSpace("Bedroom 1");
            PrimaryItem BedroomBed = new PrimaryItem("Bed", Bedroom.Name);
            PrimaryItem BedroomCloset = new PrimaryItem("Closet", Bedroom.Name);
            PrimaryItem BedroomMiniTable1 = new PrimaryItem("Mini Table 1", Bedroom.Name);

            SecondaryItem bmt1Lamp = new SecondaryItem("Table Lamp", Bedroom.Name);
            SecondaryItem bmt1Book = new SecondaryItem("A Novel??", Bedroom.Name);
            SecondaryItem bmt1Comb = new SecondaryItem("Comb", Bedroom.Name);
            BedroomMiniTable1.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { bmt1Lamp, bmt1Book, bmt1Comb });

            PrimaryItem BedroomMiniTable2 = new PrimaryItem("Mini Table 2", Bedroom.Name);
            SecondaryItem bmt2Lamp = new SecondaryItem("Lamp", Bedroom.Name);
            SecondaryItem bmt2MiniDrawer = new SecondaryItem("Mini Drawer", Bedroom.Name);
            SecondaryItem bmt2Tissue = new SecondaryItem("Tissue", Bedroom.Name);
            BedroomMiniTable2.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { bmt2Lamp, bmt2MiniDrawer, bmt2Tissue });

            PrimaryItem BedroomCarpet = new PrimaryItem("Carpet", Bedroom.Name);
            List<PrimaryItem> itemsInBedroom = new List<PrimaryItem> { BedroomBed, BedroomCloset, BedroomMiniTable1, BedroomMiniTable2, BedroomCarpet };
            Bedroom.ItemsAvailable.AddRange(itemsInBedroom);

            //Master Bedroom
            HouseSpace MasterBedroom = new HouseSpace("Master Bedroom");
            PrimaryItem MasterBedroomBed = new PrimaryItem("Bed", MasterBedroom.Name);
            PrimaryItem MasterBedroomCloset = new PrimaryItem("Closet", MasterBedroom.Name);

            SecondaryItem mbClosetTie = new SecondaryItem("Necktie", MasterBedroom.Name);
            SecondaryItem mbClosetBelt = new SecondaryItem("Leather Belt", MasterBedroom.Name);
            SecondaryItem mbClosetBox = new SecondaryItem("Locked Wooden Box", MasterBedroom.Name);
            MasterBedroomCloset.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { mbClosetTie, mbClosetBelt, mbClosetBox });

            PrimaryItem MasterBedroomMiniTable1 = new PrimaryItem("Mini Table 1", MasterBedroom.Name);
            SecondaryItem mbmt1Lamp = new SecondaryItem("Lamp", MasterBedroom.Name);
            SecondaryItem mbmt1Perfume = new SecondaryItem("Perfume", MasterBedroom.Name);
            SecondaryItem mbmt1Glasses = new SecondaryItem("Reading Glasses", MasterBedroom.Name);
            SecondaryItem mbmt1HandLotion = new SecondaryItem("Hand Lotion", MasterBedroom.Name);
            MasterBedroomMiniTable1.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { mbmt1Lamp, mbmt1Perfume, mbmt1Glasses, mbmt1HandLotion });

            PrimaryItem MasterBedroomMiniTable2 = new PrimaryItem("Mini Table 2", MasterBedroom.Name);
            SecondaryItem mbmt2Wallet = new SecondaryItem("Wallet", MasterBedroom.Name);
            SecondaryItem mbmt2Receipt = new SecondaryItem("Broken Receipt", MasterBedroom.Name);
            SecondaryItem CarKey = new SecondaryItem("Car Key", MasterBedroom.Name);
            MasterBedroomMiniTable2.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { mbmt2Wallet, mbmt2Receipt, CarKey });

            PrimaryItem MasterBedroomCarpet = new PrimaryItem("Carpet", MasterBedroom.Name);
            List<PrimaryItem> itemsInMasterBedroom = new List<PrimaryItem> { MasterBedroomBed, MasterBedroomCloset, MasterBedroomMiniTable1, MasterBedroomMiniTable2, MasterBedroomCarpet };
            MasterBedroom.ItemsAvailable.AddRange(itemsInMasterBedroom);

            //Bathroom 2
            HouseSpace Bathroom2 = new HouseSpace("Bathroom 2");
            PrimaryItem Bathroom2ToiletBowl = new PrimaryItem("Toilet Bowl", Bathroom2.Name);
            PrimaryItem Bathroom2Pail = new PrimaryItem("Pail", Bathroom2.Name);
            PrimaryItem Bathroom2Tap = new PrimaryItem("Tap", Bathroom2.Name);
            PrimaryItem Bathroom2Hose = new PrimaryItem("Hose", Bathroom2.Name);
            PrimaryItem Bathroom2Shower = new PrimaryItem("Shower", Bathroom2.Name);
            PrimaryItem Bathroom2Soap = new PrimaryItem("Soap", Bathroom2.Name);
            List<PrimaryItem> itemsInBath2 = new List<PrimaryItem> { Bathroom2ToiletBowl, Bathroom2Pail, Bathroom2Tap, Bathroom2Hose, Bathroom2Shower, Bathroom2Soap };
            Bathroom2.ItemsAvailable.AddRange(itemsInBath2);

            //Kitchen
            HouseSpace Kitchen = new HouseSpace("Kitchen");
            PrimaryItem KitchenLaundryBasket = new PrimaryItem("Laundry Basket with stack of clothes", Kitchen.Name);
            PrimaryItem KitchenFridge = new PrimaryItem("Fridge", Kitchen.Name);
            PrimaryItem KitchenOven = new PrimaryItem("Oven", Kitchen.Name);
            PrimaryItem KitchenSink = new PrimaryItem("Sink", Kitchen.Name);

            PrimaryItem KitchenBarTable = new PrimaryItem("Bar Table", Kitchen.Name);
            SecondaryItem kbtFruitBowl = new SecondaryItem("Fruit Bowl", Kitchen.Name);
            SecondaryItem kbtTissueRoll = new SecondaryItem("Kitchen Tissue Roll", Kitchen.Name);
            SecondaryItem kbtCoaster = new SecondaryItem("Drink Coaster", Kitchen.Name);
            KitchenBarTable.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { kbtFruitBowl, kbtTissueRoll, kbtCoaster });

            PrimaryItem KitchenStoveCabinet = new PrimaryItem("Stove Cabinet", Kitchen.Name);
            List<PrimaryItem> itemsInKitchen = new List<PrimaryItem> { KitchenLaundryBasket, KitchenFridge, KitchenOven, KitchenSink, KitchenBarTable, KitchenStoveCabinet };
            Kitchen.ItemsAvailable.AddRange(itemsInKitchen);

            //Living Room
            HouseSpace LivingRoom = new HouseSpace("Living Room");
            PrimaryItem LRSofa1 = new PrimaryItem("Sofa 1", LivingRoom.Name);
            PrimaryItem LRSofa2 = new PrimaryItem("Sofa 2", LivingRoom.Name);

            SecondaryItem husbandPhone = new SecondaryItem("Husband Smartphone", LivingRoom.Name);
            LRSofa2.AvailableSecondaryItem.Add(husbandPhone);

            PrimaryItem LRCoffeeTable = new PrimaryItem("Coffee Table", LivingRoom.Name);
            SecondaryItem lrctMiniVase = new SecondaryItem("Mini Vase", LivingRoom.Name);
            SecondaryItem lrctMagazine = new SecondaryItem("Fashion Magazine", LivingRoom.Name);
            SecondaryItem lrctRemote = new SecondaryItem("TV Remote", LivingRoom.Name);
            LRCoffeeTable.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { lrctMiniVase, lrctMagazine, lrctRemote });

            PrimaryItem LRTVCabinet = new PrimaryItem("TV Cabinet", LivingRoom.Name);
            SecondaryItem lrtvcCCTVWire = new SecondaryItem("CCTV wire", LivingRoom.Name);
            SecondaryItem lrtvcSwitch1 = new SecondaryItem("switch1", LivingRoom.Name);
            SecondaryItem lrtvcSwitch2 = new SecondaryItem("switch2", LivingRoom.Name);
            SecondaryItem lrtvcRouter = new SecondaryItem("Internet Router", LivingRoom.Name);
            LRTVCabinet.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { lrtvcCCTVWire, lrtvcSwitch1, lrtvcSwitch2, lrtvcRouter });

            PrimaryItem LRCCTV = new PrimaryItem("CCTV", LivingRoom.Name);
            PrimaryItem LRCarpet = new PrimaryItem("Carpet", LivingRoom.Name);
            PrimaryItem LRDiningTable = new PrimaryItem("Dining Table", LivingRoom.Name);
            PrimaryItem LRCatMiniMat = new PrimaryItem("Cat Mini Mat", LivingRoom.Name);
            List<PrimaryItem> itemsInLivingRoom = new List<PrimaryItem> { LRSofa1, LRSofa2, LRCoffeeTable, LRTVCabinet, LRCCTV, LRCarpet, LRDiningTable, LRCatMiniMat };
            LivingRoom.ItemsAvailable.AddRange(itemsInLivingRoom);

            //Garage
            HouseSpace Garage = new HouseSpace("Garage");

            PrimaryItem GarageHusbandCar = new PrimaryItem("Husband Car", Garage.Name);
            SecondaryItem HCTires = new SecondaryItem("Tires", Garage.Name);
            GarageHusbandCar.AvailableSecondaryItem.Add(HCTires);

            PrimaryItem GarageWifeCar = new PrimaryItem("Wife Car", Garage.Name);
            SecondaryItem WCTires = new SecondaryItem("Tires", Garage.Name);
            GarageWifeCar.AvailableSecondaryItem.Add(WCTires);

            PrimaryItem GarageCatCage = new PrimaryItem("Cat Cage", Garage.Name);
            SecondaryItem CageDoor = new SecondaryItem("Cage Door", Garage.Name);
            GarageCatCage.AvailableSecondaryItem.Add(CageDoor);

            PrimaryItem GarageCatFoodSack = new PrimaryItem("Cat Food Sack", Garage.Name);
            SecondaryItem sackOpening = new SecondaryItem("Sack Opening", Garage.Name);
            GarageCatFoodSack.AvailableSecondaryItem.Add(sackOpening);

            PrimaryItem GarageToolBox = new PrimaryItem("Tool Box", Garage.Name);
            PrimaryItem GarageShoeRack = new PrimaryItem("Shoe Rack", Garage.Name);
            List<PrimaryItem> itemsInGarage = new List<PrimaryItem> { GarageHusbandCar, GarageWifeCar, GarageCatCage, GarageCatFoodSack, GarageToolBox, GarageShoeRack };
            Garage.ItemsAvailable.AddRange(itemsInGarage);

            this.HouseSpaceList.Add(Bathroom1);
            this.HouseSpaceList.Add(Bedroom);
            this.HouseSpaceList.Add(MasterBedroom);
            this.HouseSpaceList.Add(Bathroom2);
            this.HouseSpaceList.Add(Kitchen);
            this.HouseSpaceList.Add(LivingRoom);
            this.HouseSpaceList.Add(Garage);
        }

        public void playGame()
        {
            this.Intro.playScene(this);
            this.Scene1.playScene(this);
            this.Scene2.playScene(this);
            this.Scene3.playScene(this);
            this.Scene4.playScene(this);
            this.Scene5.playScene(this);
            this.Scene6.playScene(this);
        }
    }
}