# FPT Science Management and International Affairs

[![CodeFactor](https://www.codefactor.io/repository/github/fpt-sap-lab/science-management-and-international-affairs/badge)](https://www.codefactor.io/repository/github/fpt-sap-lab/science-management-and-international-affairs)

## CÁC BƯỚC CHUẨN BỊ SAU KHI CLONE REPOS

### Tự động format code thừa sau khi push commit:
- Cài dotnet-format (nếu máy chưa cài): dotnet tool install -g dotnet-format
- Copy file `pre-commit` ở thư mục `FPT_Science\WebApplication\Scripts\git_hooks` sang thư mục `FPT_Science\.git\hooks`
## CONVENTION CHO GITHUB PULL REQUEST
* Pull Request sẽ chia làm các loại (TAG):</br>
        - <B>fix</B></br>
        - <B>add</B></br>
        - <B>remove</B></br>
        - <B>database</B></br>
        - <B>resources</B></br>
        - <B>config</B></br>
* Title của pull request sẽ được đặt theo format: <B>[<tên tag>]</B> - <I><Nội dung></I>
* Luôn Squashing Pull Requests giúp cho lịch sử commits được gọn

![image](https://29comwzoq712ml5vj5gf479x-wpengine.netdna-ssl.com/wp-content/uploads/2019/10/github-merge-options.png)

## CONVENTION CHO BACK-END
* Thêm Controller trực tiếp trong folder `Controllers`
* Khi tạo View thì `Add View` trực tiếp từ `return View();`
        
![image](https://user-images.githubusercontent.com/35557579/106367296-12574f00-6374-11eb-927f-65aa0cbc1203.png)

## CONVENTION CHO LAYOUT
* Luôn thêm `| FPT Science` vào title
* Chỉ thêm `Meta Descriptions` vào `<head>` của layout dành cho Guest

![image](https://user-images.githubusercontent.com/35557579/106366744-a7f0df80-6370-11eb-896e-c61a50c8cb1b.png)

## CONVENTION CHO FRONT-END
* Luôn thêm `Title` cho tất cả các màn
* Luôn thêm `Description` cho những màn quan trọng như Homepage, trang chủ quản lý khoa học, hợp tác quốc tế,...
* Không thêm `Description` cho những màn dành cho Admin

![image](https://user-images.githubusercontent.com/35557579/106367006-27cb7980-6372-11eb-9f75-f089d79b882c.png)
