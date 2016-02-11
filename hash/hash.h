#include <string>

#define Hash_table_SIZE 501

using namespace std;

class Hash
{
private:
	struct node
	{
		string data;
		int count;
		struct node* link;
		node( string key, int num, struct node* next )
		{ data = key; count = num; link = next; }
	};
	struct node* hash_table[Hash_table_SIZE];
public:
	Hash()
	{
		for( int i = 0; i < Hash_table_SIZE ; i++ )
			hash_table[i] = new node("\0", 0, NULL);	// hash table �ʱ�ȭ
	}
	void add(string key);
	unsigned int hash(string key);
};

void Hash :: add(string key)
{
	for ( int i = 0 ; i < key.length() ; i++ )
		key[i] = tolower(key[i]);	// string�� ��� ���ĺ� �ҹ��ڷ� ��ȯ

	struct node *p = hash_table[hash(key)];
	int flag = 0;

	if( p->data != "\0" )	// hash table�� ������� ���� ���
	{
		while ( p->link != NULL )
		{
			if ( p->data != key )
				p = p->link;	// �Է��� ���� hash table�� ������ ���� ���� ��� chaining ��� ��� -> ���� node�� �̵�
			else
			{ p->count++; flag++; break; }	// �Է��� ���� hash table�� ������ ���� ��� count ����
			// flag : �Է��� ���� ���� hash�� �����ϴ��� �˻��ϴ� flag
		}
	}
	else	// hash table�� ������� ��� table�� ���� �߰�
	{
		p->data = key;
		p->count = 1;
	}
	
	if ( flag == 0 ) p->link = new node(key, 1, NULL);
	// hash�� �������� ���� ���(hash table�� chain�� ��� �������� ���� ���) chaining
}

unsigned int Hash :: hash(string key)	// hash �Լ� ���� ����
{
	/*TABLE_SIZE�� hashing�� �̿�*/
	unsigned long num = Hash_table_SIZE;

	/*string key�� (hash << 5)�� ���� �߰� ��ġ ����*/
	for ( int i = 0 ; i < key.length() ; i++ )
		num = ((num << 5) + num) + key[i];

	/*TABLE_SIZE�� ���� �������� index ����*/
	return num % Hash_table_SIZE;
}