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

public:

	Queue() : front(nullptr) {}

	void Push(const string& value)
	{
		Node* new_Node = new Node{ value, nullptr };

		if (!front)
			front = new_Node;
		else
		{
			Node* current = front;
			while (current->Next)
				current = current->Next;
			current->Next = new_Node;
		}
	}

	string Check_front()
	{
		string item = front->Data;
		return item;
	}

	void Pop()
	{

		if (front) {
			Node* temp = front;
			front = front->Next; 
			delete temp;
		}
		else {
			cout << "Очередь пуста((((" << endl;
		}
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

	int Weight(const string& value)
	{
		int c = 0;
		//int x;
		Node* temp = front;
		while (temp)
		{
			if (temp->Data == value)
				return c;
			c++;
			temp = temp->Next;
		}
		return -1;
	}

	//Конструктор копирования
	Queue(const Queue& other) : front(nullptr)
	{
		Node* current = other.front;
		while (current) {
			Push(current->Data);
			current = current->Next;
		}
	}

	//Последовательное соединение
	Queue operator+(const Queue& other)
	{
		Queue result;
		Node* current = this->front;
		while (current)
		{
			result.Push(current->Data);
			current = current->Next;
		}

		current = other.front;
		while (current)
		{
			result.Push(current->Data);
			current = current->Next;
		}

		return result;
	}

	Queue operator*(const Queue& other)
	{
		Queue result;
		Node* A = this->front;
		Node* B = other.front;
		while (A || B)
		{
			if (A)
			{
				result.Push(A->Data);
				A = A->Next;
			}
			if (B)
			{
				result.Push(B->Data);
				B = B->Next;
			}
		}
		
		return result;
	}

	Queue operator-() const
	{
		Queue result;
		Node* current = front;
		while (current) {
			Node* new_Node = new Node{ current->Data, result.front };
			result.front = new_Node;
			current = current->Next; 
		}
		return result;
	}


	void display() const {
		Node* current = front;
		cout << "Очередь: ";
		while (current) {
			cout << current->Data << " ";
			current = current->Next;
		}
		cout << endl;
	}

	Node* Get()
	{
		return front;
	}

	~Queue() // удаляю все элементы для очищения памяти
	{
		Node* current = front;
		while (current != nullptr)
		{
			Node* temp = current;
			current = current->Next;
			delete temp;
		}
	}


};

Queue VtorayaKassa(Queue& kassa) //присутствует недоработка в этой функции
{
	Queue result;
	int c = 0;
	Node* current = kassa.Get();
	while (current)
	{
		c++;
		current = current->Next;
	}

	current = kassa.Get();
	int c1 = c / 2;
	for (int i = 0; i < c1;	i++)
	{
		result.Push(kassa.Check_front());
	}
	return result;
}

int main() {


	setlocale(LC_ALL, "RU");

	Queue Q1;
	Queue Q2;
	string research = "Первый";

	Q1.Push("Первый");
	Q1.Push("Второй");
	Q1.Push("Третий");
	Q1.Push("Четвертый");

	cout << Q1.Check_front() << endl;


	Q2.Push("Пятый");
	Q2.Push("Шестой");

	Queue Q3 = Q1 + Q2;
	Q3.display();

	Queue Q4 = Q1 * Q2;
	Q4.display();

	Queue Q5 = -Q1;
	Q5.display();


	cout << endl;
	cout << endl;

	Q1.display();
	Queue Q6 = VtorayaKassa(Q1);
	Q6.display();
	Q1.display();
	

	return 0;
}