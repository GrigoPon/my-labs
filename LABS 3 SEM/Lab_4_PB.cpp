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


	void PushRight(const string& value) {
		Node* newNode = new Node(value);
		newNode->Prev = Right;
		if (Right != nullptr) {
			Right->Next = newNode;
		}
		else {
			Left = newNode;
		}
		Right = newNode;
	}

	void Display() {
		if (isEmpty()) {
			cout << "Stack is empty." << endl;
			return;
		}

		Node* currentNode = Left; // Начинаем с начала стека
		cout << "Stack elements: ";

		// Обходим стек и выводим значения
		while (currentNode != nullptr) {
			cout << currentNode->Data << " "; // Выводим данные узла
			currentNode = currentNode->Next; // Переходим к следующему узлу
		}

		cout << endl; // Переводим строку после вывода
	}

	string PopRight() {
		if (Right == nullptr) {
			cout << "Пустой стек" << endl;
			return "";
		}
		Node* tempNode = Right;
		string retrieve = tempNode->Data;
		Right = Right->Prev; // Перемещаем указатель на предыдущий узел
		if (Right != nullptr) {
			Right->Next = nullptr; // Обновляем следующий указатель нового конца
		}
		else {
			Left = nullptr; // Если стек стал пустым, обнуляем также начало
		}
		return retrieve;
		delete tempNode; // Удаляем старый узел
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
	S.PushRight("Первый");
	S.PushRight("Второй");
	S.PushRight("Третий");
	S.Display();
	S.PopRight();
	S.Display();

	return 0;
}