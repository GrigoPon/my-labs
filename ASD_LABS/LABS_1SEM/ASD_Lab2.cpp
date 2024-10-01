#include <iostream>
#include <stack>
#include <vector>
#include <cmath>
#include <string>
#include <iomanip>

using namespace std;

int Prior(char op)
{
	if (op == '+' || op == '-') return 1;
	else if (op == '*' || op == '/') return 2;
	return 0;
}

double Ops(double a, double b, char op)
{
	switch (op)
	{
	case '+': { return a + b; break; }
	case '-': { return a - b; break; }
	case '*': { return a * b; break; }
	case '/': { if (b == 0) { throw runtime_error("На ноль делить нельзя!!!"); } return a / b; break; }
	
	}
	return 0;
}

double Calculation(const string &expression)
{
	stack<double> values;
	stack<char> ops;
	double k;
	

	for (int i = 0; i < expression.length(); i++)
	{
		// игнор пробелы
		if (isspace(expression[i])) continue;

		//обработка числовых значений
		
		if (isdigit(expression[i]))
		{
			double val = 0;
			while (i < expression.length() && (isdigit(expression[i]) || expression[i] == '.'))
			{
				//обработка десятичной части числа
				if (expression[i] == '.')
				{
					double decimal_part = 0.1;
					i++;
					while (i < expression.length() && isdigit(expression[i]))
					{

						val += (expression[i] - '0') * decimal_part;
						//позиционирование десятичного разряда
						decimal_part /= 10;
						i++;
					}
					continue;
				}
				//позиционирование разряда (десятки, сотни...)
				val = val * 10 + (expression[i] - '0');
				i++;
			}
			values.push(val);
			i--;
		}
		//обработка скобок
		else if (expression[i] == '(')
		{
			ops.push(expression[i]);
		}
		else if (expression[i] == ')')
		{
			while (!ops.empty() && ops.top() != '(')
			{
				double val2 = values.top();
				values.pop();

				double val1 = values.top();
				values.pop();

				char op = ops.top();
				ops.pop();

				values.push(Ops(val1, val2, op));
			}
			ops.pop();
		}
		//Обработка операций
		else if (expression[i] == '+' || expression[i] == '-' || expression[i] == '*' || expression[i] == '/')
		{
			while (!ops.empty() && Prior(ops.top()) >= Prior(expression[i]))
			{
				double val2 = values.top();
				values.pop();

				double val1 = values.top();
				values.pop();

				char op = ops.top();
				ops.pop();

				values.push(Ops(val1, val2, op));
			}
			ops.push(expression[i]);
		}
		else if (expression[i] == '=')
		{
			while (!ops.empty())
			{
				double val2 = values.top();
				values.pop();

				double val1 = values.top();
				values.pop();

				char op = ops.top();
				ops.pop();

				values.push(Ops(val1, val2, op));
			}
		}
	}
	k = values.top();
	return k;
}





int main()
{
	setlocale(LC_ALL, "RU");

	string expression;

	cout << "Введите математическое выражение (окончание - знак '='): ";
	getline(cin, expression);
	string exp = expression;

	if (expression.empty() || !expression.back() == '=')
	{ 
		cerr << "Ошибка! Выражение должно оканчиваться на '='" << endl;
		return 1;
	}

	try
	{
		double result = Calculation(expression);
		cout << "Ответ: ";
		cout << /*fixed << setprecision(3) <<*/ result << endl;
	}
	catch (const runtime_error &e)
	{
		cerr << "Ошибка! " << e.what() << endl;
	}



	return 0;
}