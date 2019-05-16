
use qltv
alter proc sp_MuonSach @isbn int,@madocgia int
as
	declare @macuonsach_chuaduocmuon int,
			@slSach_Nguoilon int,
			@slSach_TreEm int,
			@slSach_TongCong int,
			@soluongsach_conlai int

	if not exists (select * from Muon where isbn=@isbn and ma_docgia = @madocgia )
	begin
	--lay ma cuon sach dau tien co tinh trang Y
		declare @macuonsach int
		select @macuonsach= Ma_CuonSach
		from CuonSach
		where @isbn =isbn
		and TinhTrang = 'Y'
		order by Ma_CuonSach

		if exists (select * from NguoiLon where ma_docgia = @madocgia )
		begin
			select @soluongsach_conlai = Count(*)
			from CuonSach
			where @isbn = isbn
			and TinhTrang = 'Y'
			if  @soluongsach_conlai>0
				begin
					select @slSach_Nguoilon = Count(*)
					from NguoiLon,Muon
					where @madocgia = NguoiLon.ma_docgia
					and Muon.ma_docgia = NguoiLon.ma_docgia

					declare @matreem int
					select  @matreem = TreEm.ma_docgia
					from NguoiLon,TreEm
					where NguoiLon.ma_docgia = @madocgia
					and NguoiLon.ma_docgia = TreEm.ma_docgia_nguoilon
					order by TreEm.ma_docgia

					select @slSach_TreEm = Count(*)
					from Muon
					where ma_docgia = @matreem

					set @slSach_TongCong = @slSach_Nguoilon+@slSach_TreEm
					if(@slSach_TongCong<5)
					begin
						--tiến hành mượn sách
						insert into Muon(isbn,ma_cuonsach,ma_docgia,ngayGio_muon,ngay_hethan)
						values(@isbn,@macuonsach,@madocgia,getdate(),getdate())
						return
						update CuonSach
						set TinhTrang='N'
						where Ma_CuonSach=@macuonsach

						if(@soluongsach_conlai=1)
						update DauSach
						set trangthai='N'
						where isbn = @isbn
					end
					else
						raiserror ('vuot qua gioi han qui dinh',16,1)
				end
			else
				begin
					--Đăng ký--
					insert into DangKy(isbn,ma_docgia,ngaygio_dk,ghichu)
					values(@isbn,@madocgia,getdate(),'')
					return
				end

			

			
		end
		else
		begin
			select @soluongsach_conlai = Count(*)
			from CuonSach
			where @isbn = isbn
			and TinhTrang = 'Y'
			if  @soluongsach_conlai>0
			begin
				if not exists (select * from Muon where ma_docgia = @madocgia)
				begin
						
						--tiến hành mượn sách
						insert into Muon(isbn,ma_cuonsach,ma_docgia,ngayGio_muon,ngay_hethan)
						values(@isbn,@macuonsach,@madocgia,getdate(),getdate())
						return

						update CuonSach
						set TinhTrang='N'
						where Ma_CuonSach=@macuonsach

						if(@soluongsach_conlai=1)
						update DauSach
						set trangthai='N'
						where isbn = @isbn

				end
				else
				begin
					raiserror('da vuot qua gioi han muon theo QD tre em',16,1)
				end
			end
			else
			begin
				--Đăng ký--
				insert into DangKy(isbn,ma_docgia,ngaygio_dk,ghichu)
				values(@isbn,@madocgia,getdate(),'')
				return
			end
		end
	end
	else
	--độc giả đã mượn cuốn sách này rồi
	begin
		print 'doc gia '+ str(@madocgia) + ' da muon dau sach ' + str(@isbn) + ' roi'
		return
	end

	
			
			

exec sp_MuonSach 1,1

insert into Muon(isbn,ma_cuonsach,ma_docgia,ngayGio_muon,ngay_hethan)
			values(1,1,1,getdate(),getdate())


select * from dangky
select * from Muon where ma_docgia=94
select * from TreEm where ma_docgia=94 and ma_docgia in (select ma_docgia from Muon) order by ma_docgia
select Ma_CuonSach from CuonSach where isbn=1 and tinhtrang = 'Y' order by Ma_CuonSach

exec sp_MuonSach 1,96


select * from CuonSach where isbn =1
