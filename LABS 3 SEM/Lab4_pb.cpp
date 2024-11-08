#include <iostream>
#include <string>
#include <iomanip>
#include <stack>
#include <vector>
#include <algorithm>

using namespace std;

struct Node {
	string Data;
	Node* Right;
	Node* Left;

	Node(const string& value) : Data(value), Right(nullptr), Left(nullptr) {}

	~Node() {

		Right = nullptr;
		Left = nullptr;
	}
};


struct Stack {
private:
	Node* Head;

public:

	Stack() : Head {} {}

	bool isEmpty() const {
		return Head == nullptr;
	}


	void Push(const string& value) {
		Node* newNode = new Node(value);
		newNode->Right = Head;
		if (Head != nullptr) {
			Head->Left = newNode;
		}
		Head = newNode;
	}

	void print() const {
		Node* current = Head;
		while (current != nullptr) {
			cout << current->Data << " ";
			current = current->Right;
		}
		cout << endl;
	}

	string Pop() {
		if (isEmpty()) {
			cout << "Пустой стек" << endl;
		}
		while (Head->Left != nullptr)
		{
			Head = Head->Left;
		}
		Node* tempNode = Head;
		Head = Head->Right;
		if (Head != nullptr) {
			Head->Left = nullptr;
		}
		return tempNode->Data;
		delete tempNode;
	}

	string Check_right() {
		if (Head == nullptr)
		{
			cout << "Стек пуст!" << endl;
			return "";
		}
		else
		{
			string Last = Head->Data;
			return Last;
		}
	}

	bool is_Found(const string& value)
	{
		Node* temp = Head;
		while (temp)
		{
			if (temp->Data == value)
				return true;
			temp = temp->Left;
		}
		return false;
	}

	int Weight(const string& value)
	{
		int c = 1;
		//int x;
		Node* temp;
		while (Head->Right != nullptr)
		{
			Head = Head->Right;
		}
		temp = Head;
		while (temp != nullptr)
		{
			if (temp->Data == value)
				return c;
			c++;
			temp = temp->Left	;
		}
		return -1;
	}

	int Stack_size()
	{
		int c = 0;
		if (Head == nullptr)
		{
			cout << "Стек пуст!" << endl;
			return 0;
		}
		else
		{
			Node* temp = Head;
			while (temp)
			{
				c++;
				temp = temp->Right;
			}
			return c;
		}
	}

	Stack(const Stack& other) : Head(nullptr) {
		Node* current = other.Head->Right;
		while (current != nullptr) {
			Push(current->Data);
			current = current->Left;
		}
	}

	
	void StackSort() {
		if (isEmpty()) return;

		vector<string> elements;
		
		Node* current = Head;
		while (current) {
			elements.push_back(Pop());
			current = current->Right;
		}

		sort(elements.begin(), elements.end());


		for (auto element : elements) {
			Push(element);
		}
	}
 

	Stack operator+(const Stack& other) {
		Stack result;
		Node* temp = this->Head->Right;
		while (temp != nullptr)
		{
			result.Push(temp->Data);
			temp = temp->Left;
		}
		Node* current = other.Head->Right;
		while (current != nullptr) {
			result.Push(current->Data);
			current = current->Left;
		}
		return result;
	}

	Stack operator*(const Stack& other) const {
		Stack result;
		Node* current1 = this->Head->Right;
		Node* current2 = other.Head->Right;

		while (current1 != nullptr && current2 != nullptr) {
			result.Push(current1->Data);
			result.Push(current2->Data);
			current1 = current1->Left;
			current2 = current2->Left;
		}

		while (current1 != nullptr) {
			result.Push(current1->Data);
			current1 = current1->Left;
		}
		while (current2 != nullptr) {
			result.Push(current2->Data);
			current2 = current2->Left;
		}

		return result;
	}

	Stack operator-() const {
		Stack result;
		Node* current = this->Head;
		while (current)
		{
			result.Push(current->Data);
			current = current->Right;
		}
		return result;
	}


	~Stack() {
		while (Head != nullptr) {
			Node* tempNode = Head;
			Head = Head->Right;     
			if (Head != nullptr) {
				Head->Left = nullptr;
			}

			delete tempNode;
		}
		Head = nullptr;
	}

};



int main()
{
	setlocale(LC_ALL, "RU");
	Stack S;
	string str = "Второй";
	cout << "Глубина стека: " << S.Stack_size() << endl;
	S.Push("Первый");
	S.Push("Второй");
	S.Push("Третий");
	S.print();

	cout << "Позиция элемента '" << str << "': " << S.Weight(str) << endl;


	if (S.is_Found(str))
		cout << "Элемент '" << str << "' найден." << endl;
	else
		cout << "Элемент '" << str << "' не найден." << endl;
	
	S.Pop();
	S.print();
	cout << "Просмотр элемента из начала стека: " << S.Check_right() << endl;



	if (S.is_Found(str))
		cout << "Элемент '" << str << "' найден." << endl;
	else
		cout << "Элемент '" << str << "' не найден." << endl;

	cout << endl;
	cout << endl;


	Stack S2;
	S2.Push("Третий");
	S2.Push("Четвертый");

	Stack S3 = S2;
	S3.print();

	Stack S4 = S + S2;
	S4.print();

	Stack S5 = S2 * S;
	S5.print();

	Stack S7 = -S4;
	S7.print();
	
	Stack S6;
	S6.Push("Яблоко");
	S6.Push("Ежевика");
	S6.Push("Апельсин");
	S6.Push("Орех");

	cout << endl;
	cout << "Сортировка: " << endl;
	S6.StackSort();
	S6.print();


	return 0;
}
