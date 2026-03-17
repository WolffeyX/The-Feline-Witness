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
        //Properties
        public House house;
        public MainCharacter mainCharacterCat;
        public Character husband;
        public Character wife;
        public Character mistress;
        public List<HouseSpace> HouseSpaceList = new List<HouseSpace>();
        public IntroScene intro;
        public Scene1 scene1;

        public GameEngine()//No issue (understood)
        {
            house = new House();
            mainCharacterCat = new MainCharacter();
            husband = new Character();
            wife = new Character();
            mistress = new Character();
            this.SetupHouseItems(); // Call the method to setup all the items in the house here
            intro = new IntroScene("Intro");
            scene1 = new Scene1("Scene 1");
        }

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
            Bathroom1.itemsAvailable.AddRange(itemsInBath1);

            //Bedroom
            HouseSpace Bedroom = new HouseSpace("Bedroom 1");
            PrimaryItem BedroomBed = new PrimaryItem("Bed", Bedroom.Name);
            PrimaryItem BedroomCloset = new PrimaryItem("Closet", Bedroom.Name);
            PrimaryItem BedroomMiniTable1 = new PrimaryItem("Mini Table 1", Bedroom.Name);
            //Add some secondary item to the mini table 1
            SecondaryItem bmt1Lamp = new SecondaryItem("Table Lamp", Bedroom.Name);
            SecondaryItem bmt1Book = new SecondaryItem("A Novel??", Bedroom.Name);
            SecondaryItem bmt1Comb = new SecondaryItem("Comb", Bedroom.Name);
            BedroomMiniTable1.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { bmt1Lamp, bmt1Book, bmt1Comb });
            PrimaryItem BedroomMiniTable2 = new PrimaryItem("Mini Table 2", Bedroom.Name);
            //Add some secondary item to the mini table 2
            SecondaryItem bmt2Lamp = new SecondaryItem("Lamp", Bedroom.Name); // Item wajib
            SecondaryItem bmt2MiniDrawer = new SecondaryItem("Mini Drawer", Bedroom.Name);
            SecondaryItem bmt2Tissue = new SecondaryItem("Tissue", Bedroom.Name);
            BedroomMiniTable2.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { bmt2Lamp, bmt2MiniDrawer, bmt2Tissue });
            PrimaryItem BedroomCarpet = new PrimaryItem("Carpet", Bedroom.Name);
            List<PrimaryItem> itemsInBedroom = new List<PrimaryItem> { BedroomBed, BedroomCloset, BedroomMiniTable1, BedroomMiniTable2, BedroomCarpet };
            Bedroom.itemsAvailable.AddRange(itemsInBedroom);

            //Master Bedroom
            HouseSpace MasterBedroom = new HouseSpace("Master Bedroom");
            PrimaryItem MasterBedroomBed = new PrimaryItem("Bed", MasterBedroom.Name);
            PrimaryItem MasterBedroomCloset = new PrimaryItem("Closet", MasterBedroom.Name);
            //Add some secondary item to the closet
            SecondaryItem mbClosetTie = new SecondaryItem("Necktie", MasterBedroom.Name);
            SecondaryItem mbClosetBelt = new SecondaryItem("Leather Belt", MasterBedroom.Name);
            SecondaryItem mbClosetBox = new SecondaryItem("Locked Wooden Box", MasterBedroom.Name);
            MasterBedroomCloset.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { mbClosetTie, mbClosetBelt, mbClosetBox });
            PrimaryItem MasterBedroomMiniTable1 = new PrimaryItem("Mini Table 1", MasterBedroom.Name);
            //Add some secondary item to the mini table 1
            SecondaryItem mbmt1Lamp = new SecondaryItem("Lamp", MasterBedroom.Name);
            SecondaryItem mbmt1Perfume = new SecondaryItem("Perfume", MasterBedroom.Name);
            SecondaryItem mbmt1Glasses = new SecondaryItem("Reading Glasses", MasterBedroom.Name);
            SecondaryItem mbmt1HandLotion = new SecondaryItem("Hand Lotion", MasterBedroom.Name);
            MasterBedroomMiniTable1.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { mbmt1Lamp, mbmt1Perfume, mbmt1Glasses, mbmt1HandLotion });
            PrimaryItem MasterBedroomMiniTable2 = new PrimaryItem("Mini Table 2", MasterBedroom.Name);
            //Add some secondary item to the mini table 2
            SecondaryItem mbmt2Wallet = new SecondaryItem("Wallet", MasterBedroom.Name);
            SecondaryItem mbmt2Receipt = new SecondaryItem("Broken Receipt", MasterBedroom.Name);
            SecondaryItem CarKey = new SecondaryItem("Car Key", MasterBedroom.Name);
            MasterBedroomMiniTable2.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { mbmt2Wallet, mbmt2Receipt, CarKey });
            PrimaryItem MasterBedroomCarpet = new PrimaryItem("Carpet", MasterBedroom.Name);
            List<PrimaryItem> itemsInMasterBedroom = new List<PrimaryItem> { MasterBedroomBed, MasterBedroomCloset, MasterBedroomMiniTable1, MasterBedroomMiniTable2, MasterBedroomCarpet };
            MasterBedroom.itemsAvailable.AddRange(itemsInMasterBedroom);

            //Bathroom 2
            HouseSpace Bathroom2 = new HouseSpace("Bathromm 2");
            PrimaryItem Bathroom2ToiletBowl = new PrimaryItem("Toilet Bowl", Bathroom2.Name);
            PrimaryItem Bathroom2Pail = new PrimaryItem("Pail", Bathroom2.Name);
            PrimaryItem Bathroom2Tap = new PrimaryItem("Tap", Bathroom2.Name);
            PrimaryItem Bathroom2Hose = new PrimaryItem("Hose", Bathroom2.Name);
            PrimaryItem Bathroom2Shower = new PrimaryItem("Shower", Bathroom2.Name);
            PrimaryItem Bathroom2Soap = new PrimaryItem("Soap", Bathroom2.Name);
            List<PrimaryItem> itemsInBath2 = new List<PrimaryItem> { Bathroom2ToiletBowl, Bathroom2Pail, Bathroom2Tap, Bathroom2Hose, Bathroom2Shower, Bathroom2Soap };
            Bathroom2.itemsAvailable.AddRange(itemsInBath2);

            //Kitchen
            HouseSpace Kitchen = new HouseSpace("Kitchen");
            PrimaryItem KitchenLaundryBasket = new PrimaryItem("Laundry Basket", Kitchen.Name);
            PrimaryItem KitchenFridge = new PrimaryItem("Fridge", Kitchen.Name);
            PrimaryItem KitchenOven = new PrimaryItem("Oven", Kitchen.Name);
            PrimaryItem KitchenSink = new PrimaryItem("Sink", Kitchen.Name);
            PrimaryItem KitchenBarTable = new PrimaryItem("Bar Table", Kitchen.Name);
            //Add some secondary item to the bar table
            SecondaryItem kbtFruitBowl = new SecondaryItem("Fruit Bowl", Kitchen.Name);
            SecondaryItem kbtTissueRoll = new SecondaryItem("Kitchen Tissue Roll", Kitchen.Name);
            SecondaryItem kbtCoaster = new SecondaryItem("Drink Coaster", Kitchen.Name);
            KitchenBarTable.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { kbtFruitBowl, kbtTissueRoll, kbtCoaster });
            PrimaryItem KitchenStoveCabinet = new PrimaryItem("Stove Cabinet", Kitchen.Name);
            List<PrimaryItem> itemsInKitchen = new List<PrimaryItem> { KitchenLaundryBasket, KitchenFridge, KitchenOven, KitchenSink, KitchenBarTable, KitchenStoveCabinet };
            Kitchen.itemsAvailable.AddRange(itemsInKitchen);

            //Living Room
            HouseSpace LivingRoom = new HouseSpace("Living Room");
            PrimaryItem LRSofa1 = new PrimaryItem("Sofa 1", LivingRoom.Name);
            PrimaryItem LRSofa2 = new PrimaryItem("Sofa 2", LivingRoom.Name);
            // Item wajib untuk Scene 1:
            SecondaryItem husbandPhone = new SecondaryItem("Husband Smartphone", LivingRoom.Name);
            LRSofa2.AvailableSecondaryItem.Add(husbandPhone);
            PrimaryItem LRCoffeeTable = new PrimaryItem("Coffee Table", LivingRoom.Name);
            //Add some secondary item to the coffee table
            SecondaryItem lrctMiniVase = new SecondaryItem("Mini Vase", LivingRoom.Name);
            SecondaryItem lrctMagazine = new SecondaryItem("Fashion Magazine", LivingRoom.Name);
            SecondaryItem lrctRemote = new SecondaryItem("TV Remote", LivingRoom.Name);
            LRCoffeeTable.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { lrctMiniVase, lrctMagazine, lrctRemote });
            PrimaryItem LRTVCabinet = new PrimaryItem("TV Cabinet", LivingRoom.Name);
            //Add some secondary item to the TV cabinet
            SecondaryItem lrtvcCCTVWire = new SecondaryItem("CCTV wire", LivingRoom.Name); // Item wajib
            SecondaryItem lrtvcSwitch1 = new SecondaryItem("switch1", LivingRoom.Name); // Item wajib
            SecondaryItem lrtvcSwitch2 = new SecondaryItem("switch2", LivingRoom.Name); // Item wajib
            SecondaryItem lrtvcRouter = new SecondaryItem("Internet Router", LivingRoom.Name); // Tambahan
            LRTVCabinet.AvailableSecondaryItem.AddRange(new List<SecondaryItem> { lrtvcCCTVWire, lrtvcSwitch1, lrtvcSwitch2, lrtvcRouter });
            PrimaryItem LRCCTV = new PrimaryItem("CCTV", LivingRoom.Name);
            PrimaryItem LRCarpet = new PrimaryItem("Carpet", LivingRoom.Name);
            PrimaryItem LRDiningTable = new PrimaryItem("Dining Table", LivingRoom.Name);
            PrimaryItem LRCatMiniMat = new PrimaryItem("Cat Mini Mat", LivingRoom.Name);
            List<PrimaryItem> itemsInLivingRoom = new List<PrimaryItem> { LRSofa1, LRSofa2, LRCoffeeTable, LRTVCabinet, LRCCTV, LRCarpet, LRDiningTable, LRCatMiniMat };
            LivingRoom.itemsAvailable.AddRange(itemsInLivingRoom);

            //Garage
            HouseSpace Garage = new HouseSpace("Garage");
            PrimaryItem GarageHusbandCar = new PrimaryItem("HusbandCar", Garage.Name);
            PrimaryItem GarageWifeCar = new PrimaryItem("WifeCar", Garage.Name);
            PrimaryItem GarageCatCage = new PrimaryItem("Cat Cage", Garage.Name);
            PrimaryItem GarageCatFoodSack = new PrimaryItem("Cat Food Sack", Garage.Name);
            PrimaryItem GarageToolBox = new PrimaryItem("Tool Box", Garage.Name);
            PrimaryItem GarageShoeRack = new PrimaryItem("Shoe Rack", Garage.Name);
            List<PrimaryItem> itemsInGarage = new List<PrimaryItem> { GarageHusbandCar, GarageWifeCar, GarageCatCage, GarageCatFoodSack, GarageToolBox, GarageShoeRack };
            Garage.itemsAvailable.AddRange(itemsInGarage);

            HouseSpaceList.Add(Bathroom1);
            HouseSpaceList.Add(Bedroom);
            HouseSpaceList.Add(MasterBedroom);
            HouseSpaceList.Add(Bathroom2);
            HouseSpaceList.Add(Kitchen);
            HouseSpaceList.Add(LivingRoom);
            HouseSpaceList.Add(Garage);
        }

        public void playGame()
        {
            intro.playScene(this);
            scene1.playScene(this);
        }
    }
}
