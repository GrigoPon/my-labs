#include <iostream>
#include <ostream>
#include <string>

using namespace std;

float pi = 3.14;

class R_Complex {
    float _r;
    float _y;
    
public:
    R_Complex() {
        _r = 1;
        _y = 1;
    }

    R_Complex(float r, float y) {
        _r = r;
        _y = y;
    }

    R_Complex(const R_Complex& p) {
        _r = p._r;
        _y = p._y;
    }

    void SetR(float r) {
        _r = r;
    }

    void SetY(float y) {
        _y = y;
    }
    
    R_Complex& operator=(R_Complex& p) {
        this->_r = p._r;
        this->_y = p._y;
        return *this;
    }

    R_Complex& operator+(const R_Complex& p) {
        R_Complex temp(sqrt(this->_r * this->_r + p._r * p._r + 2 * this->_r * p._r * cos(abs(this->_y - p._r))), (atan(this->_r * sin(this->_y) + p._r * sin(p._y)) / (this->_r * cos(this->_y) + p._r * cos(p._y))));
        return temp;
    }

    R_Complex& operator++(int value) {
        R_Complex temp(*this);
        _r++;
        return temp;
    }

    R_Complex& operator++() {
        _r++;
        return *this;
    }

    R_Complex& operator*(const R_Complex& a) {
        R_Complex temp(this->_r * a._r, this->_y + a._y);
        return temp;
    }

    R_Complex& operator +=(R_Complex& p) {
        _r = sqrt(this->_r * this->_r + p._r * p._r - 2 * this->_r * p._r * cos(abs(this->_y - p._y)));
        float tan = (this->_r * sin(this->_y) + p._r * sin(p._y)) / (this->_r * cos(this->_y) + p._r * cos(p._y)); // мнимая часть комплексных чисел деленная на действительную часть комплексных чисел
        if ((this->_r * cos(this->_y) + p._r * cos(p._y)) == 0) { // действительная часть комплексного числа (проверка тангенса, если обращается в бесконечность -> присвоение угла)
            if ((this->_r * sin(this->_y) + p._r * sin(p._y)) > 0) {
                _y = pi / 2;
            }
            else {
                _y = 3 * pi / 2;
            }
        }
        else {
            _y = atan(tan);
            if ((this->_r * cos(this->_y) + p._r * cos(p._y)) < 0) {
                _y += pi;
            }
        }

        return *this;
    }

    R_Complex& operator*=(const R_Complex& a) {
        _r = this->_r * a._r;
        _y = this->_y + a._y;
        return *this;
    }


    float GetR() {
        return this->_r;
    }
    float GetY() {
        return this->_y;
    }


    ~R_Complex() {
        cout << "class deleted" << endl;
    }
};

float ScalarC(R_Complex a, R_Complex b) {
    
    float temp = abs(a.GetR()* b.GetR()* cos(abs(a.GetY() - b.GetY())));
    return temp;
}

R_Complex& AbsC(R_Complex& a, R_Complex& b)
{
    if (a.GetR() >= b.GetR())
        return a;
    else
        return b;
}

int qC(R_Complex a) {
    float angle = a.GetY();
    
    string temp;
    if (angle > 2 * pi) {
        while (angle > 2 * pi) {
            angle -= 2 * pi;
        }
    }
    if (angle < 2 * pi) {
        while (angle < 2 * pi) {
            angle += 2 * pi;
        }
    }
    if (angle < pi) {
        if (angle < pi / 2)
            return 1;
        else
            return 2;
    }
    else {
        if (angle < 3 * pi / 2)
            return 3;
        else
            return 4;
    }
}



ostream& operator<< (ostream& os, R_Complex& a) {
    cout << endl;
    return os << "радиус-вектор: " << a.GetR() << " угол компл. числа: " << a.GetY() << endl;
}

istream& operator >> (istream& in, R_Complex& a) {
    float _r;
    float _y;
    in >> _r >> _y;
    a.SetR(_r);
    a.SetY(_y);
    return in;
}




int main()
{
    setlocale(LC_ALL, "RU");

    R_Complex a = {};
    R_Complex b = {};
    cout << "Введите первое комплексное число: (r, y) ";
    cin >> a;
    cout << "Введите второе комплексное число: (r, y) ";
    cin >> b;
    cout << "первое комплексное число: " << a << endl;
    cout << "второе комплексное число: " << b << endl;
    cout << endl;
    cout << endl;
    cout << endl;
    R_Complex c;
    c = a++;
    cout << a << endl;
    cout << c << endl;

    a.operator+=(b);
    cout << a << endl;


    //скалярное произведение
    cout << "scalar multiply" << endl;
    cout << ScalarC(a, b) << endl;

    //максимальное по модулю
    R_Complex d;
    d = AbsC(a, b);
    cout << d << endl;

    //Квадрант комплексных чисел а и б
    cout << "Число а находится в " << qC(a) << endl;
    cout << "Число б находится в " << qC(b) << endl;
}

