#include <iostream>
#include <string>
#include <stack>

using namespace std;

int main() {
	
	setlocale(LC_ALL, "RU");
	/*stack <int> storage;
	string A;
	cout << "Введите скобочную последовательность: " << endl;
	cin >> A;
	int s = A.size();
	for (int i = 0; i < s; i++)
	{
		if (A[i] == '(' || A[i] == '{' || A[i] == '[')
			storage.push(A[i]);
		else
		{
			if (A[i] == ')' && storage.size() != 0 && storage.top() == '(')
				storage.pop();
			else if (A[i] == '}' && storage.size() != 0 && storage.top() == '{')
				storage.pop();
			else if (A[i] == ']' && storage.size() != 0 && storage.top() == '[')
				storage.pop();
			else
			{
				storage.push(i);
				break;
			}

		}
		
	}
	if (storage.size() == 0)
		cout << "Последовательность скобок верная!" << endl;
	else
		cout << "Последовательность скобок неверная!!!" << endl;*/

	string A = "";
	cout << "Введите скобочную последовательность: " << endl;
	cin >> A;
	string* storage = new string[10];
	int j = 0;

	int s = A.size();
	for (int i = 0; i < s; i++)
	{
		if (A[i] == '(' || A[i] == '[' || A[i] == '{')
		{
			storage[j] = A[i];
			j++;
		}
		else
		{
			if (j != 0 && A[i] == ')' && storage->size() != 0 && storage[j - 1] == "(")
				j--;
			else if (j != 0 && A[i] == ']' && storage->size() != 0 && storage[j - 1] == "[")
				j--;
			else if (j != 0 && A[i] == '}' && storage->size() != 0 && storage[j - 1] == "{")
				j--;
			else
			{
				j++;
				break;
			}
		}
	}

	if (j != 0)
		cout << "Скобочная последовательность неправильная!!!" << endl;
	
	else
		cout << "Скобочная последовательность правильная!" << endl;





	return 0;
}
