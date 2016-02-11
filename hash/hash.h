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
			hash_table[i] = new node("\0", 0, NULL);	// hash table 초기화
	}
	void add(string key);
	unsigned int hash(string key);
};

void Hash :: add(string key)
{
	for ( int i = 0 ; i < key.length() ; i++ )
		key[i] = tolower(key[i]);	// string의 모든 알파벳 소문자로 변환

	struct node *p = hash_table[hash(key)];
	int flag = 0;

	if( p->data != "\0" )	// hash table이 비어있지 않을 경우
	{
		while ( p->link != NULL )
		{
			if ( p->data != key )
				p = p->link;	// 입력할 값과 hash table의 내용이 같지 않을 경우 chaining 기법 사용 -> 다음 node로 이동
			else
			{ p->count++; flag++; break; }	// 입력할 값과 hash table의 내용이 같을 경우 count 증가
			// flag : 입력할 값이 현재 hash에 존재하는지 검사하는 flag
		}
	}
	else	// hash table이 비어있을 경우 table에 정보 추가
	{
		p->data = key;
		p->count = 1;
	}
	
	if ( flag == 0 ) p->link = new node(key, 1, NULL);
	// hash에 존재하지 않을 경우(hash table과 chain에 모두 존재하지 않을 경우) chaining
}

unsigned int Hash :: hash(string key)	// hash 함수 변경 가능
{
	/*TABLE_SIZE를 hashing에 이용*/
	unsigned long num = Hash_table_SIZE;

	/*string key에 (hash << 5)을 통해 추가 위치 연산*/
	for ( int i = 0 ; i < key.length() ; i++ )
		num = ((num << 5) + num) + key[i];

	/*TABLE_SIZE로 나눈 나머지로 index 추출*/
	return num % Hash_table_SIZE;
}