#include <iostream>
#include <string>
#include <iomanip>

using namespace std;

struct Node {
	string Data;
	Node* Next;
	Node* Prev;

	Node(const string& value) : Data(value), Prev(nullptr), Next(nullptr) {}
};


struct Stack {
private:
	Node* Left;
	Node* Right;

public:

	Stack() : Left(nullptr), Right(nullptr) {}

	bool isEmpty() const {
		return Right == nullptr;
	}


	void PushLeft(const string& value) {
		Node* newNode = new Node(value);
		newNode->Next = Left;
		if (Left != nullptr) {
			Left->Prev = newNode;
		}
		else {
			Right = newNode;
		}
		Left = newNode;
	}

	void print() const {
		Node* current = Left;
		while (current != nullptr) {
			std::cout << current->Data << " ";
			current = current->Next;
		}
		std::cout << std::endl;
	}

	void PopLeft() {
		if (Right == nullptr) {
			cout << "Пустой стек" << endl;
		}
		Node* tempNode = Left;
		//string retrieve = tempNode->Data;
		Left = Left->Next; // Перемещаем указатель на предыдущий узел
		if (Left != nullptr) {
			Left->Prev = nullptr; // Обновляем следующий указатель нового конца
		}
		else {
			Right = nullptr; // Если стек стал пустым, обнуляем также начало
		}
		delete tempNode; // Удаляем старый узел
	}

	string Check_right() {
		if (Left == nullptr)
		{
			cout << "Стек пуст!" << endl;
			return "";
		}
		else
		{
			string Last = Left->Data;
			return Last;
		}
	}

	bool is_Found(const string& value)
	{
		Node* temp = Left;
		while (temp)
		{
			if (temp->Data == value)
				return true;
			temp = temp->Next;
		}
		return false;
	}

	int Weight(const string& value)
	{
		int c = 1;
		//int x;
		Node* temp = Right;
		while (temp)
		{
			if (temp->Data == value)
				return c;
			c++;
			temp = temp->Prev;
		}
		return -1;
	}

	int Stack_size()
	{
		int c = 0;
		if (Left == nullptr)
		{
			cout << "Стек пуст!" << endl;
			return 0;
		}
		else
		{
			Node* temp = Left;
			while (temp)
			{
				c++;
				temp = temp->Next;
			}
			return c;
		}
	}

	Stack(const Stack& other) : Left(nullptr), Right(nullptr) {
		Node* current = other.Left;
		while (current != nullptr) {
			PushLeft(current->Data);
			current = current->Next;
		}
	}


	Stack operator+(const Stack& other) {
		Stack result;
		Node* temp = this->Right;
		while (temp != nullptr)
		{
			result.PushLeft(temp->Data);
			temp = temp->Prev;
		}
		Node* current = other.Right;
		while (current != nullptr) {
			result.PushLeft(current->Data);
			current = current->Prev;
		}
		return result;
	}

	Stack operator*(const Stack& other) const {
		Stack result;
		Node* current1 = this->Right;
		Node* current2 = other.Right;

		while (current1 != nullptr && current2 != nullptr) {
			result.PushLeft(current1->Data);
			result.PushLeft(current2->Data);
			current1 = current1->Prev;
			current2 = current2->Prev;
		}

		while (current1 != nullptr) {
			result.PushLeft(current1->Data);
			current1 = current1->Prev;
		}
		while (current2 != nullptr) {
			result.PushLeft(current2->Data);
			current2 = current2->Prev;
		}

		return result;
	}

	Stack operator-() const {
		Stack result;
		Node* current = this->Left;
		while (current)
		{
			result.PushLeft(current->Data);
			current = current->Next;
		}
		return result;
	}


	~Stack() {
		while (Left != nullptr) {
			Node* tempNode = Left;
			Left = Left->Next;
			delete tempNode;
		}
	}
};



int main()
{
	setlocale(LC_ALL, "RU");
	Stack S;
	string str = "Третий";
	cout << "Глубина стека: " << S.Stack_size() << endl;
	S.PushLeft("Первый");
	S.PushLeft("Второй");
	S.PushLeft("Третий");
	S.print();
	cout << "Позиция элемента '" << str << "': " << S.Weight(str) << endl;;
	if (S.is_Found(str))
		cout << "Элемент '" << str << "' найден." << endl;
	else
		cout << "Элемент '" << str << "' не найден." << endl;
	S.PopLeft();
	S.print();
	cout << "Просмотр элемента из начала стека: " << S.Check_right() << endl;

	if (S.is_Found(str))
		cout << "Элемент '" << str << "' найден." << endl;
	else
		cout << "Элемент '" << str << "' не найден." << endl;

	cout << endl;
	cout << endl;
	Stack S2;
	S2.PushLeft("Третий");
	S2.PushLeft("Четвертый");
	S2.print();
	Stack S3 = S + S2;
	S3.print();
	Stack S4 = S * S2;
	S4.print();
	Stack S5 = -S4;
	S5.print();
	return 0;
}
