﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game
{
    enum Move { STALL, FORWARD, BACKWARD, LEFT, RIGHT };
    enum iName { NOTE, PHONE, AUDIO, SECRETCASE}; //Used to index into ITEMS
    static class Constants
    {
        public const int FLOOR_LENGTH = 4; //X
        public const int FLOOR_WIDTH = 4; //Y
        public const int NUM_FLOORS = 10; 
        public const int NUM_ITEMS = 4;
        static public String[] ITEMS = new String[] { "Note", "Phone", "Audio", "Secret Case" };
        static public Random randGen = new Random();
    }

    class Graphic //simulate graphics (for now just text)
    {

        private char[,] image;
        private Coordinate pCoord; //player Coordinate
        private ArrayList marks; //array list of NamedCoord's
        private String text;

        public Graphic(Coordinate pCoord, ArrayList marks, String text)
        {
            this.pCoord = new Coordinate(pCoord.x, pCoord.y);
            this.marks = new ArrayList(marks);

            this.image = new char[Constants.FLOOR_LENGTH,Constants.FLOOR_WIDTH];

            for (int i = 0; i < Constants.FLOOR_LENGTH; i++)
                for (int j = 0; j < Constants.FLOOR_WIDTH; j++)
                    image[i, j] = '-';


            foreach (NamedCoord mark in marks)
            {
                if (mark.name == "CorrectElevator")
                    image[mark.coord.x, mark.coord.y] = 'e';
                else
                    image[mark.coord.x, mark.coord.y] = 'o';
            }

            this.image[pCoord.x, pCoord.y] = 'X'; //may overwrite a mark if on the same coordinate

            this.text = text;
        }

        public Graphic(String text)
        {
            this.image = new char[Constants.FLOOR_LENGTH, Constants.FLOOR_WIDTH];

            for (int i = 0; i < Constants.FLOOR_LENGTH; i++)
                for (int j = 0; j < Constants.FLOOR_WIDTH; j++)
                    image[i, j] = '-';

            this.text = text;
        }

        public String Text
        {
            set { this.text = value; }
            get { return text; }
        }

        public void setImage(Coordinate pCoord, ArrayList marks)
        {
            //Reset
            foreach (NamedCoord mark in this.marks)
                this.image[mark.coord.x, mark.coord.y] = '-'; //reset old marks

            this.image[this.pCoord.x, this.pCoord.y] = '-';

            this.marks = new ArrayList(marks);
            this.pCoord = new Coordinate(pCoord.x, pCoord.y);

            //Fill
            foreach (NamedCoord mark in marks)
            {
                if (mark.name == "CorrectElevator")
                    image[mark.coord.x, mark.coord.y] = 'e';
                else
                    image[mark.coord.x, mark.coord.y] = 'o';
            }

            this.image[pCoord.x, pCoord.y] = 'X'; //may overwrite an 'O' if on the same coordinate
        }

        public void flashlight(Coordinate pCoord, ArrayList marks)
        {
            //Reset floor to dashes
            foreach (NamedCoord mark in this.marks)
                this.image[mark.coord.x, mark.coord.y] = '-';
         
            //Reset player coords to dashes
            this.image[this.pCoord.x, this.pCoord.y] = '-';

            this.marks = new ArrayList(marks);
            this.pCoord = new Coordinate(pCoord.x, pCoord.y);

            foreach (NamedCoord mark in marks)
            {
                //ABOVE
                if (mark.coord.x == pCoord.x && mark.coord.y == pCoord.y + 1 )
                {
                    if (mark.name == "CorrectElevator")
                        this.image[mark.coord.x, mark.coord.y] = 'e';
                    else
                        this.image[mark.coord.x, mark.coord.y] = 'o';
                }
                //BELOW
                if (mark.coord.x == pCoord.x && mark.coord.y == pCoord.y - 1)
                {
                    if (mark.name == "CorrectElevator")
                        this.image[mark.coord.x, mark.coord.y] = 'e';
                    else
                        this.image[mark.coord.x, mark.coord.y] = 'o';
                }
                //TO THE RIGHT
                if (mark.coord.x == pCoord.x + 1 && mark.coord.y == pCoord.y)
                {
                    if (mark.name == "CorrectElevator")
                        this.image[mark.coord.x, mark.coord.y] = 'e';
                    else
                        this.image[mark.coord.x, mark.coord.y] = 'o';
                }
                //TO THE LEFT
                if (mark.coord.x == pCoord.x - 1 && mark.coord.y == pCoord.y)
                {
                    if (mark.name == "CorrectElevator")
                        this.image[mark.coord.x, mark.coord.y] = 'e';
                    else
                        this.image[mark.coord.x, mark.coord.y] = 'o';
                }
            }

            this.image[pCoord.x, pCoord.y] = 'X'; //may overwrite an 'O' if on the same coordinate
        }


        public String getImage()
        { 
            String image_t = "";
            for (int i = 0; i < Constants.FLOOR_LENGTH; i++)
            {
                for (int j = 0; j < Constants.FLOOR_WIDTH; j++)
                {
                    image_t += this.image[i, j] + "     ";
                }

                image_t += System.Environment.NewLine;
            }

            return image_t;
        }
    }

    //just helper class to pass coordinates around easier.
    class Coordinate
    {
        public int x;
        public int y;
        public Coordinate(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

    }

    class NamedCoord
    {
        public String name;
        public Coordinate coord;
        public NamedCoord(String name, Coordinate coord)
        {
            this.name = name;
            this.coord = new Coordinate(coord.x, coord.y);
        }

        //Useful when removing from ArrayLists
        public override bool Equals(Object obj)
        {
            if (obj == null || (!(obj is NamedCoord) && !(obj is String)))
                return false;
            
            if(obj is NamedCoord)
                return this.name == ((NamedCoord)obj).name;
            
            return this.name == ((String)obj);
        }

        public override int GetHashCode()
        {
            return this.name.GetHashCode();
        }
    }

    class PassCode
    {
        public int a, b, c;
        public PassCode(int a, int b, int c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }
    }

    class GameState
    {
        public String playerName;
        public int floorNumber;
        public PassCode pc;
        public Coordinate coord;
        public bool caseLocked; //is the case locked? IF they don't have it, its a yes
        public bool[] have;

        public GameState(String playerName, int floorNumber,
                         PassCode pc, Coordinate coord, bool caseLocked,
                         bool[] have)
        {
            this.playerName = playerName;
            this.floorNumber = floorNumber;
            this.pc = pc;
            this.coord = coord;
            this.caseLocked = caseLocked;

            this.have = new bool[Constants.NUM_ITEMS];
            
            for(int i = 0; i < Constants.NUM_ITEMS; i++)
                this.have[i] = have[i];

        }

        public GameState(String playerName) //default constructor with an inital state
        {
            this.playerName = playerName;
            floorNumber = Constants.NUM_FLOORS;
            pc = null;
            coord = new Coordinate(0, 0);
            caseLocked = true;

            this.have = new bool[Constants.NUM_ITEMS];

            //set all too false, player doesn't have anything
            for (int i = 0; i < Constants.NUM_ITEMS; i++)
                this.have[i] = false;
        }
    }

    class Tile
    {
        private Item item = null;

        public Tile()
        {
            //Not sure how to decide how item is initialized
            //Random random = new Random();
            /*
            int rand = Constants.randGen.Next(0, 4); //(0,4]
            if (rand == 3) item = null; //No item
            else item = new Tool(Constants.ITEMS[rand], "No hint!"); //Item with no hint
            */
        }

        public Item Item
        {
            set { this.item = value; }
            get { return this.item; }
        }

    }

    //can't be instantiated!
    abstract class Item
    {
        private String itemName;
        protected String itemHint;
        public Item(String name, String hint)
        {
            itemName = name;
            itemHint = hint;//String.Copy(hint);
        }

        public String name() { return itemName; }
        abstract public String getHint();
    }

    //Regular Items
    class Tool : Item
    {
        public Tool(String name, String hint) : base(name, hint) { }
        override public String getHint() { return itemHint; }
    }

    //Special Item, a case
    class Case : Item
    {
        private PassCode pc; //passcode
        private bool locked;
        public Case(String name, String hint, PassCode pc, bool locked)
            : base(name, hint) //call base class constructor
        {
            this.pc = pc;
            this.locked = locked;
        }

        public bool tryToUnlock(PassCode pc)
        {
            if (this.pc.a == pc.a && this.pc.b == pc.b && this.pc.c == pc.c)
            {
                this.locked = false;
                return true;
            }

            return false;
        }

        public bool isLocked() { return locked; }

        override public String getHint()
        {
            if (!locked) return itemHint;
            return "Case Locked!";
        }
    }

    //initialize an array of elevators that will change floors for the player
    abstract class Elevator
    {
        protected int x, y, floor;

        public Elevator(int i, int j, int floor)
        {
            this.x = i;
            this.y = j;
            this.floor = floor;
        }

        public Boolean isThereElevator(int i, int j){ return x==i && y==j; }

        //Will make sure an elevator doesnt pass its boundary limits
        public Boolean canGoUp() { return this.floor != Constants.NUM_FLOORS; }

        public Boolean canGoDown() { return this.floor != 1; }

        public Coordinate getCoord() { return new Coordinate(this.x, this.y); }

        public abstract int go_up();
        public abstract int go_down();
    }

    class WrongElevator : Elevator
    {
        protected int randAbove;    //randomly takes user to any floor above current floor
        protected int randBelow;    //will not take user to first floor

        public WrongElevator(int i, int j, int floor) : base (i, j, floor) { }

        public override int go_up()
        {
            //randAbove = Constants.randGen.Next(this.floor, Constants.NUM_FLOORS);
            randAbove = this.floor + 1;
            return randAbove;
        }

        public override int go_down()
        {
            //randBelow = Constants.randGen.Next(2, this.floor);     //avoids a random ride to first floor
            randBelow = this.floor - 1;
            return randBelow;
        }
    }

    class CorrectElevator : Elevator
    {
        protected int lastFloor, nextFloor;

        public CorrectElevator(int i, int j, int floor) : base (i, j, floor) { }

        public void setPattern(int last, int next)
        {
            this.lastFloor = last;
            this.nextFloor = next;
        }

        public override int go_up() { return lastFloor; }

        public override int go_down() { return nextFloor; }
    }
}