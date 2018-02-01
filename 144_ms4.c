//Lyall Alex Sinclair-Barrett
//036251106

#include <stdio.h>
//# using namespace std;

#define LINEAR 1
#define FORM 0
#define STOCK 1
#define CHECKOUT 0
#define MAX_ITEM_NO 500
#define MAX_QTY 999
#define SKU_MAX 999
#define SKU_MIN 100
#define DATAFILE "items.txt"

const double tax = 0.13;

struct Item //item structure to be used
{
	double price;
	int sku;
	int isTaxed;
	int quantity;
	int minQuantity;
	char name[21];
};

void welcome(void);
void printTitle(void);
void printFooter(double gTotal);
void flushKeyboard(void);
void pause(void);
int getInt(void);
int getIntLimited(int lowerLimit, int upperLimit);
double getDouble(void);
double getDoubleLimited(double lowerLimit, double upperLimit);
int yes(void);
int menu(void);
void GroceryInventorySystem(void);
double totalAfterTax(struct Item item);
int isLowQuantity(struct Item item);
struct Item itemEntry(int sku);
void displayItem(struct Item item, int linear);
void listItems(const struct Item item[], int noOfItems);
int locateItem(const struct Item item[], int NoOfRecs, int sku, int *index);
void search(const struct Item item[], int NoOfRecs);
void updateItem(struct Item *itemptr);
void addItem(struct Item item[], int *NoOfRecs, int sku);
void addOrUpdateItem(struct Item item[], int *NoOfRecs);
void adjustQuantity(struct Item item[], int NoOfRecs, int stock);
void saveItem(struct Item item, FILE *dataFile);
int loadItem(struct Item *item, FILE* dataFile);
int saveItems(const struct Item item[], char fileName[], int NoOfRecs);
int loadItems(struct Item item[], char fileName[], int* NoOfRecsPtr);
void checkSave(const struct Item item[], char fileName[], int NoOfRecs);


int main(void) //call GorceryInventorySystem, it handles the main program
{
	GroceryInventorySystem();

	return 0;
}

void welcome(void) //Print at start of program
{
	printf("---=== Grocery Inventory System ===---");
	printf("\n");
	printf("\n");
}

void printTitle(void) //Print layout header
{
	printf("Row |SKU| Name               | Price  |Taxed| Qty | Min |   Total    |Atn\n");
	printf("----+---+--------------------+--------+-----+-----+-----+------------|---\n");
}

void printFooter(double gTotal) //print footer
{
	printf("--------------------------------------------------------+----------------\n");
	if (gTotal > 0)
	{
		printf("                                           Grand Total: |%12.2lf\n", gTotal);
	}
}

void flushKeyboard(void) // flush the buffer, find \n
{
	char entered;
	scanf("%c", &entered);

	while (entered != '\n')
	{
		scanf("%c", &entered);
	}
}

void pause(void) //pause system
{
	printf("Press <ENTER> to continue...");
	flushKeyboard();
}

int getInt(void) //find and store entered integer, rejects characters
{
	int numEntered;
	char charEntered;

	scanf("%d%c", &numEntered, &charEntered);

	while (charEntered != '\n')
	{
		flushKeyboard();
		printf("Invalid integer, please try again: ");
		scanf("%d%c", &numEntered, &charEntered);
	}
	return numEntered;
}

int getIntLimited(int lowerLimit, int upperLimit) //Find an integer within a range, uses getInt() for validation
{
	int checkValue = 0;
	checkValue = getInt();
	while (checkValue < lowerLimit || checkValue > upperLimit)
	{
		printf("Invalid value, %d < value < %d: ", lowerLimit, upperLimit);
		checkValue = getInt();
	}
	return checkValue;
}

double getDouble(void) //find and store entered double, rejects character
{
	double numEntered = 0;
	char charEntered;

	scanf("%lf%c", &numEntered, &charEntered);

	while (charEntered != '\n')
	{
		flushKeyboard();
		printf("Invalid number, please try again: ");
		scanf("%lf%c", &numEntered, &charEntered);
	}
	return numEntered;
}

double getDoubleLimited(double lowerLimit, double upperLimit) // find double withing range, uses getDouble() for validation
{
	double checkValue;
	checkValue = getDouble();
	while (checkValue < lowerLimit || checkValue > upperLimit)
	{
		printf("Invalid value, %lf< value < %lf: ", lowerLimit, upperLimit);
		checkValue = getDouble();
	}
	return checkValue;
}

int yes(void) // ask user for yes or no response, returns 1 if YES
{
	int toReturn = 0;
	char entered;

	scanf("%c", &entered);
	flushKeyboard();

	while (entered != 'Y' && entered != 'y' && entered != 'N' && entered != 'n')
	{
		printf("Only (Y)es or (N)o are acceptable: ");
		scanf("%c", &entered);
		flushKeyboard();
	}
	if (entered == 'Y' || entered == 'y')
	{
		toReturn = 1;
	}
	return toReturn;
}

int menu(void) // display the main menu, ask for user input
{
	int numEntered;

	printf("1- List all items\n");
	printf("2- Search by SKU\n");
	printf("3- Checkout an item\n");
	printf("4- Stock an item\n");
	printf("5- Add new item or update item\n");
	printf("6- delete item\n");
	printf("7- Search by name\n");
	printf("0- Exit program\n");
	printf("> ");

	numEntered = getIntLimited(0, 7);
	return numEntered;
}

void GroceryInventorySystem(void) // main system
{
	int userEntered;
	int isDone = 0;
	struct Item myItems[MAX_ITEM_NO];
	int numRecs;
	welcome();

	if (loadItems(myItems, DATAFILE, &numRecs) == 1) //if loadItems returns 1(true/success)
	{
		while (isDone == 0) //while exit program(0) has not been selected
		{
			userEntered = menu(); //call main mene, returns slected menu item

			switch (userEntered)
			{
			case 1:
				listItems(myItems, numRecs);
				pause();
				break;
			case 2:
				search(myItems, numRecs);
				pause();
				break;
			case 3:
				adjustQuantity(myItems, numRecs, CHECKOUT);
				checkSave(myItems, DATAFILE, numRecs);
				pause();
				break;
			case 4:
				adjustQuantity(myItems, numRecs, STOCK);
				checkSave(myItems, DATAFILE, numRecs);
				pause();
				break;
			case 5:
				addOrUpdateItem(myItems, &numRecs);
				checkSave(myItems, DATAFILE, numRecs);
				pause();
				break;
			case 6:
				printf("Delete Item under construction!\n");
				pause();
				break;
			case 7:
				printf("Search by name under construction!\n");
				pause();
				break;
			case 0:
				printf("Exit the program? (Y)es/(N)o: ");
				isDone = yes();
				break;
			default:
				break;
			}
		}
	}
	else
	{
		printf("Could not read from %s.\n", DATAFILE); // display error if load fails
	}
}

double totalAfterTax(struct Item item) //if item is taxed return total with tax, else return price
{
	double toReturn = 0;
	toReturn = item.price * item.quantity;

	if (item.isTaxed == 1)
	{
		toReturn += toReturn * tax;
	}
	return toReturn;
}

int isLowQuantity(struct Item item) //check if quantity is less than min quantity
{
	int toReturn = 0;

	if (item.quantity <= item.minQuantity)
	{
		toReturn = 1;
	}
	return toReturn;
}

struct Item itemEntry(int sku) //create an item using an SKU
{
	struct Item newItem;

	newItem.sku = sku;
	printf("        SKU: %d\n", newItem.sku);
	printf("       Name: ");
	scanf("%20[^\n]", newItem.name);
	flushKeyboard();

	printf("      Price: ");
	newItem.price = getDoubleLimited(0.01,1000);

	printf("   Quantity: ");
	newItem.quantity = getIntLimited(1, MAX_QTY);

	printf("Minimum Qty: ");
	newItem.minQuantity = getIntLimited(0, 100);

	printf("   Is Taxed: ");
	newItem.isTaxed = yes();

	return newItem;
}

void displayItem(struct Item item, int linear) //display passed item
{
	if (linear == LINEAR) //display in a single line
	{
		if (item.isTaxed == 0) //if isTaxed false
		{
			printf("|%3d|%-20s|%8.2lf|   No| %3d | %3d |%12.2lf|", item.sku, item.name, item.price, item.quantity, item.minQuantity, totalAfterTax(item));
		}
		else //if isTaxed true
		{
			printf("|%3d|%-20s|%8.2lf|  Yes| %3d | %3d |%12.2lf|", item.sku, item.name, item.price, item.quantity, item.minQuantity, totalAfterTax(item));
		}

		if (isLowQuantity(item) == 1)//check if * is necessary
		{
			printf("***\n");
		}
		else
		{
			printf("\n");
		}
		
	}
	else if (linear == FORM) //display in stacked form, multi lines
	{
		printf("        SKU: %d\n", item.sku);
		printf("       Name: %s\n", item.name);
		printf("      Price: %.2lf\n", item.price);
		printf("   Quantity: %d\n", item.quantity);
		printf("Minimum Qty: %d\n", item.minQuantity);

		if (item.isTaxed == 0)
		{
			printf("   Is Taxed: No\n");
		}
		else
		{
			printf("   Is Taxed: Yes\n");
		}

		if (isLowQuantity(item) == 1)
		{
			printf("WARNING: Quantity low, please order ASAP!!!\n");
		}
	}
	else
	{
		printf("I take one or zero, stupid human\n");
	}
}

void listItems(const struct Item item[], int noOfItems) //display all items
{
	int i;
	double grandTotal = 0;
	printTitle();

	for (i = 0; i < noOfItems; i++)
	{
		printf("%-4d", i + 1);
		displayItem(item[i], LINEAR);
		grandTotal += totalAfterTax(item[i]);
	}
	printFooter(grandTotal);
}

int locateItem(const struct Item item[], int NoOfRecs, int sku, int *index) //check if item already exists in a passed array of items
{
	int toReturn = 0;
	int i;
	int flag = -1;

	for(i = 0; i < NoOfRecs; i++)
	{
		if (item[i].sku == sku)
		{
			*index = i;
			flag = i;
			i = NoOfRecs;
		}
	}

	if (flag > -1)
	{
		toReturn = 1;
	}
	return toReturn;
}


void search(const struct Item item[], int NoOfRecs) //using locateItem(), find and display item in passed array
{
	int flag = -1;
	int numEntered;

	printf("Please enter the SKU: ");
	
	numEntered = getIntLimited(SKU_MIN, SKU_MAX);
	
	if (locateItem(item, NoOfRecs, numEntered, &flag) == 1)
	{
		displayItem(item[flag], FORM);
	}
	else
	{
		printf("Item not found!\n");
	}

}

void updateItem(struct Item *itemptr)// using itemEntry() update existing item
{
	struct Item tempItem;

	printf("Enter new data:\n");

	tempItem = itemEntry((*itemptr).sku);

	printf("Overwrite old data? (Y)es/(N)o: ");
	if (yes() == 1)
	{
		*itemptr = tempItem;
		printf("--== Updated! ==--\n");
	}
	else
	{
		printf("--== Aborted! ==--\n");
	}
}

void addItem(struct Item item[], int *NoOfRecs, int sku) //using itemEntry() create a new item
{

	struct Item tempItem;

	if (*NoOfRecs >= MAX_ITEM_NO)
	{
		printf("Can not add new item; Storage Full!\n");
	}
	else
	{
		tempItem = itemEntry(sku);
		printf("Add Item? (Y)es/(N)o: ");
		if (yes() == 1)
		{
			printf("--== Added! ==--\n");
			item[*NoOfRecs] = tempItem;
			*NoOfRecs += 1;
		}
		else
		{
			printf("--== Aborted! ==--\n");
		}
	}
}

void addOrUpdateItem(struct Item item[], int *NoOfRecs) //using locateItem() and addItem, either find and update an item or create a new item
{
	int numEnterted = 0;
	int tempIndex = -1;

	printf("Please enter the SKU: ");
	numEnterted = getIntLimited(SKU_MIN, SKU_MAX);

	if(locateItem(item, *NoOfRecs, numEnterted, &tempIndex) == 1)
	{
		displayItem(item[tempIndex], FORM);

		printf("Item already exists, Update? (Y)es/(N)o: ");
		if (yes() == 1)
		{
			updateItem(&item[tempIndex]);
		}
		else
		{
			printf("--== Aborted! ==--\n");
		}
	}
	else
	{	
		addItem(item, NoOfRecs, numEnterted);
	}
}

void adjustQuantity(struct Item item[], int NoOfRecs, int stock) //find an item, if stock == STOCK -> stock the item, else -> checkout. Check if quantity is low
{
	int numEntered = 0;
	int tempIndex = -1;
	int qtyEntered = -1;

	printf("Please enter the SKU: ");


	numEntered = getIntLimited(SKU_MIN, SKU_MAX);

	if (locateItem(item, NoOfRecs, numEntered, &tempIndex) == 1)
	{
		displayItem(item[tempIndex], FORM);

		if (stock == STOCK)
		{
			printf("Please enter the quantity %s; Maximum of %d or 0 to abort: ", "to stock", MAX_QTY - item[tempIndex].quantity);
			qtyEntered = getIntLimited(0, MAX_QTY - item[tempIndex].quantity);
			if (qtyEntered > 0)
			{
				item[tempIndex].quantity += qtyEntered;
				printf("--== Stocked! ==--\n");
			}
		}
		else
		{
			printf("Please enter the quantity %s; Maximum of %d or 0 to abort: ", "to checkout", item[tempIndex].quantity);
			qtyEntered = getIntLimited(0, item[tempIndex].quantity);
			if (qtyEntered > 0)
			{
				item[tempIndex].quantity -= qtyEntered;
				printf("--== Checked out! ==--\n");
			}
		}
		if (qtyEntered == 0)
		{
			printf("--== Aborted! ==--\n");
		}
		else if (isLowQuantity(item[tempIndex]) == 1)
		{
			printf("Quantity is low, please reorder ASAP!!!\n");
		}
	}
	else
	{
		printf("SKU not found in storage!\n");
	}
}

void saveItem(struct Item item, FILE *dataFile) // save an item to the file
{
	fprintf(dataFile, "%d,%d,%d,%.2lf,%d,%s\n", item.sku, item.quantity, item.minQuantity, item.price, item.isTaxed, item.name);
}

int loadItem(struct Item *item, FILE* dataFile) //load an item from the file, if successful -> return 1
{
	int toReturn = 0;
	struct Item tempItem;

	if(fscanf(dataFile, "%d,%d,%d,%lf,%d,%20[^\n]", &tempItem.sku, &tempItem.quantity, &tempItem.minQuantity, &tempItem.price, &tempItem.isTaxed, tempItem.name) == 6)
	{
		*item = tempItem;
		toReturn = 1;
	}

	return toReturn;
}

int saveItems(const struct Item item[], char fileName[], int NoOfRecs) //using saveItem() save an array of items, return 1 on success
{
	FILE *myFile;
	int toReturn = 0;
	int i;

	myFile = fopen(fileName, "w");
	if (myFile != NULL)
	{
		for (i = 0; i < NoOfRecs; i++)
		{
			saveItem(item[i], myFile);
		}
		fclose(myFile);
		toReturn = 1;
	}

	return toReturn;
}

int loadItems(struct Item item[], char fileName[], int* NoOfRecsPtr)// using loadItem(), load multiple items
{
	int toReturn = 0;
	int i;
	FILE *myFile;

	myFile = fopen(fileName, "r");
	if (myFile != NULL)
	{
		i = 0;
		while(loadItem(&item[i], myFile) == 1)
		{
			i++;
		}

		*NoOfRecsPtr = i;
		fclose(myFile);
		toReturn = 1;
	}
	
	return toReturn;
}

void checkSave(const struct Item item[], char fileName[], int NoOfRecs) //display error on failed save update
{
	if (saveItems(item, fileName, NoOfRecs) == 0)
	{
		printf("could not update data file %s\n", fileName);
	}
}





