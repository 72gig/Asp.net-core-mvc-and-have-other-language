import csv
import sys


def create_csv_file(arg1, arg2, arg3):
    with open("test1.csv", 'w') as file:
        csvwriter = csv.writer(file)
        csvwriter.writerow([arg1,arg2,arg3])
    return "success"

if __name__ == '__main__':
    create_csv_file(sys.argv[1], sys.argv[2], sys.argv[3])