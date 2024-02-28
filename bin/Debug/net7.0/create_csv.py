import csv
import sys


def create_csv_file(arg1, arg2, arg3, arg4, arg5, arg6, arg7):
    with open("計算資料.csv", 'w') as file:
        csvwriter = csv.writer(file)
        # csvwriter.writerow(["類型" + arg1, "計算開始日期" + arg2, "計算結束日期" + arg3])
        # csvwriter.writerow(["總花費" + arg4, "總數量" + arg5])
        # csvwriter.writerow([r"客戶/廠商最大花費" + arg6, r"客戶/廠商最大數量" + arg7])
        csvwriter.writerow(["類型: " + arg1])
        csvwriter.writerow(["計算開始日期: " + arg2])
        csvwriter.writerow(["計算結束日期: " + arg3])
        csvwriter.writerow(["總金額: " + arg4])
        csvwriter.writerow(["總數量: " + arg5])
        csvwriter.writerow([r"客戶/廠商最大金額: " + arg6])
        csvwriter.writerow([r"客戶/廠商最大數量: " + arg7])
    return "success"

if __name__ == '__main__':
    create_csv_file(sys.argv[1], sys.argv[2], sys.argv[3],
                    sys.argv[4], sys.argv[5], sys.argv[6],
                    sys.argv[7])