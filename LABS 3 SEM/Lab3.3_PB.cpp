#include <iostream>
#include <string>

using namespace std;

struct Node {

	string Data;
	Node* Next;

};

struct Queue {
private:

	Node* front;
	Node* back;

public:

	Queue() : front(nullptr), back(nullptr) {}

	void Push(const string& value)
	{
		Node* new_Node = new Node{ value, nullptr };
		
		if (back)
			back->Next = new_Node;
		back = new_Node;
		
		//Если пустая очередь, так же указывает на новый элемент
		if (!front)
			front = new_Node;

	}

	void Сheck_Pop()
	{

		if (!front)
		{
			cerr << "Очередь пуста!!!" << endl;
			return;
		}
		

		Node* temp = front;
		front = front->Next;

		if (!front)
			back = nullptr;

		delete temp;
	}

	bool Searching(const string& value)
	{

		Node* temp = front;
		while (temp)
		{
			if (temp->Data == value)
				return true;
			temp = temp->Next;
		}

		return false;

	}

	void Check() const {
		Node* current = front;
		if (!current) {
			std::cout << "Очередь пуста!" << std::endl;
			return;
		}
		while (current) {
			std::cout << current->Data << " ";
			current = current->Next;
		}
		std::cout << std::endl;
	}


	~Queue() // удаляю все элементы для очищения памяти
	{
		while (front)
			Сheck_Pop();
	}


};

int main() {

	setlocale(LC_ALL, "RU");

	Queue Q;

	Q.Push("Первый");
	Q.Push("Второй");
	Q.Push("Третий");
	//Q.Searching("Первый");
	Q.Check();

	Q.Сheck_Pop();
	//Q.Searching("Первый");
	Q.Check();


	return 0;
}