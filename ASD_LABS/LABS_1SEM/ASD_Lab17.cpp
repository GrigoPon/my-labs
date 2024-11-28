#include <iostream>
#include <sstream>
#include <string>

using namespace std;

struct TreeNode {
    int value;
    TreeNode* Left;
    TreeNode* Right;

    TreeNode(int val) : value(val), Left(nullptr), Right(nullptr) {}
};

// Функция для создания дерева
TreeNode* parseTree(istringstream& tree) {
    if (tree.peek() == ' ') {
        tree.get(); // Пропустить пробел
    }

    if (tree.peek() == ')' || tree.peek() == ',' || tree.peek() == EOF) {
        return nullptr;
    }

    int value;
    tree >> value;
    TreeNode* node = new TreeNode(value);

    if (tree.peek() == ' ') {
        tree.get(); // Пропускать пробелы
    }

    if (tree.peek() == '(') {
        tree.get(); // Пропустить '('
        node->Left = parseTree(tree);

        if (tree.peek() == ',') {
            tree.get(); // Пропустить ','
            node->Right = parseTree(tree);
        }
        tree.get(); // Пропустить ')'
    }

    return node;
}

// Функция для печати дерева в линейно-скобочной записи
void printTree(TreeNode* root) {
    if (!root) return;

    cout << root->value; // выводим первый узел

    if (root->Left || root->Right) {
        cout << " (";
        if (root->Left) printTree(root->Left);
        else cout << " ";

        cout << ", ";
        if (root->Right) printTree(root->Right);
        else cout << " ";

        cout << ")";
    }
}

// Поиск элемента в дереве
bool search(TreeNode* root, int key) {
    if (!root) return false;
    if (root->value == key) return true;
    if (key < root->value) return search(root->Left, key);
    return search(root->Right, key);
}

// Добавление элемента в дерево
TreeNode* insert(TreeNode* root, int key) {
    if (!root) return new TreeNode(key);
    if (key < root->value)
        root->Left = insert(root->Left, key);
    else if (key > root->value)
        root->Right = insert(root->Right, key);
    return root;
}

// Удаление элемента из дерева
TreeNode* deleteNode(TreeNode* root, int key) {
    if (!root) return root;

    if (key < root->value) {
        root->Left = deleteNode(root->Left, key);
    }
    else if (key > root->value) {
        root->Right = deleteNode(root->Right, key);
    }
    else {
        if (!root->Left) {
            TreeNode* temp = root->Right;
            delete root;
            return temp;
        }
        else if (!root->Right) {
            TreeNode* temp = root->Left;
            delete root;
            return temp;
        }

        TreeNode* temp = root->Right;
        while (temp && temp->Left) temp = temp->Left;
        root->value = temp->value;
        root->Right = deleteNode(root->Right, root->value);
    }
    return root;
}

// Функция меню
void menu(TreeNode*& root) {
    while (true) {
        cout << "Меню:" << endl;
        cout << "1. Поиск элемента" << endl;
        cout << "2. Добавление элемента" << endl;
        cout << "3. Удаление элемента" << endl;
        cout << "4. Выход и вывод дерева" << endl;
        cout << "Выберите 1-4: ";

        int P, value;
        cin >> P;

        switch (P) {
        case 1:
            cout << "Введите значение для поиска: ";
            cin >> value;
            if (search(root, value))
                cout << "Элемент найден" << endl;
            else
                cout << "Элемент не найден" << endl;
            break;
        case 2:
            cout << "Введите значение для добавления: ";
            cin >> value;
            root = insert(root, value);
            cout << "Элемент добавлен" << endl;
            break;
        case 3:
            cout << "Введите значение для удаления: ";
            cin >> value;
            root = deleteNode(root, value);
            cout << "Элемент удален" << endl;
            break;
        case 4:

            cout << "Дерево в линейно - скобочной записи : ";
            printTree(root);
            cout << endl;
            return;
        default:
            cout << "Неверный выбор. Попробуйте снова." << endl;
        }
    }
}

int main() {
    setlocale(LC_ALL, "RU");
    string Example = "8 (3 (1, 6 (4, 7)), 10 (, 14 (13,)))";
    istringstream tree(Example);
    TreeNode* root = parseTree(tree);

    menu(root);

    return 0;
}