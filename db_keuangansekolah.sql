-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Waktu pembuatan: 25 Jun 2026 pada 17.14
-- Versi server: 10.6.7-MariaDB
-- Versi PHP: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `db_keuangansekolah`
--

-- --------------------------------------------------------

--
-- Struktur dari tabel `dashboard`
--

CREATE TABLE `dashboard` (
  `total_spp` varchar(50) DEFAULT '0',
  `total_tabungan` varchar(50) DEFAULT '0',
  `total_lainnya` varchar(50) DEFAULT '0'
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data untuk tabel `dashboard`
--

INSERT INTO `dashboard` (`total_spp`, `total_tabungan`, `total_lainnya`) VALUES
('20500000', '300000', '7080000'),
('20500000', '300000', '7080000'),
('20500000', '300000', '7080000');

-- --------------------------------------------------------

--
-- Struktur dari tabel `data_siswa`
--

CREATE TABLE `data_siswa` (
  `nis` varchar(10) NOT NULL,
  `nama` varchar(100) NOT NULL,
  `gender` varchar(10) NOT NULL,
  `kelas` varchar(20) NOT NULL,
  `no_hp` varchar(50) NOT NULL,
  `alamat` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data untuk tabel `data_siswa`
--

INSERT INTO `data_siswa` (`nis`, `nama`, `gender`, `kelas`, `no_hp`, `alamat`) VALUES
('0156374120', 'Siti Sulistini', 'Perempuan', 'X TKJ 1', '085345667212', 'Bojonegoro'),
('0156374121', 'Anggara Putra', 'Laki-Laki', 'X TKJ 1', '085883215467', 'Solo'),
('0156374122', 'Aldi', 'Laki-Laki', 'X TKJ 1', '085213578319', 'Lamongan'),
('0156374123', 'Muhammad Arif Maulana', 'Laki-Laki', 'X TKJ 1', '085122567451', 'Tuban'),
('0156374124', 'Muhammad Nur Aziz', 'Laki-Laki', 'X TKJ 1', '085124734587', 'Semarang'),
('0156374130', 'Fuad Dewa Ramdani', 'Laki-Laki', 'X TKJ 2', '08537577145', 'Lamongan'),
('0156374140', 'rahma', 'Perempuan', 'X TKJ 3', '085245665321', 'Surabaya');

-- --------------------------------------------------------

--
-- Struktur dari tabel `data_staff`
--

CREATE TABLE `data_staff` (
  `nip` varchar(10) NOT NULL,
  `nama` varchar(50) NOT NULL,
  `gender` varchar(10) NOT NULL,
  `jabatan` varchar(50) NOT NULL,
  `no_hp` varchar(50) NOT NULL,
  `alamat` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data untuk tabel `data_staff`
--

INSERT INTO `data_staff` (`nip`, `nama`, `gender`, `jabatan`, `no_hp`, `alamat`) VALUES
('0981234110', 'Aminudin S.Pd', 'Laki-laki', 'Guru', '085722566789', 'Surabaya'),
('0981234111', 'Supriyati M.Pd', 'Perempuan', 'Guru', '085247822345', 'Tuban'),
('0981234112', 'Yeni Triyanti S.Pd', 'Perempuan', 'Guru', '085263748882', 'Lamongan'),
('0981234113', 'Solikin M.Pd', 'Laki-laki', 'Guru', '086384623612', 'Sidoarjo'),
('0981234114', 'Ema Maemunah S.Pd', 'Perempuan', 'Guru', '081537482735', 'Bandung'),
('0981234115', 'Budi Utomo M.Pd', 'Laki-laki', 'Guru', '087265417645', 'Surabaya'),
('0981234116', 'Sulaiman S.Pd', 'Laki-laki', 'Guru', '087635478987', 'Lamongan'),
('0981234117', 'Siti Nurjanah S.Pd', 'Perempuan', 'Guru', '087658765412', 'Gresik'),
('0981234118', 'Ade Santoso M.Pd', 'Laki-laki', 'Guru', '085679243651', 'Tuban'),
('0981234119', 'Amelia Putri M.Pd', 'Perempuan', 'Guru', '089764512352', 'Gresik'),
('0981234120', 'Dini Mardianah S.Pd', 'Perempuan', 'Guru', '086538761523', 'Bojonegoro');

-- --------------------------------------------------------

--
-- Struktur dari tabel `login`
--

CREATE TABLE `login` (
  `username` varchar(100) NOT NULL,
  `password` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data untuk tabel `login`
--

INSERT INTO `login` (`username`, `password`) VALUES
('admin', 'admin123');

-- --------------------------------------------------------

--
-- Struktur dari tabel `pembayaran`
--

CREATE TABLE `pembayaran` (
  `id_pembayaran` int(11) NOT NULL,
  `nis` varchar(50) NOT NULL,
  `nama` varchar(100) NOT NULL,
  `kelas` varchar(20) NOT NULL,
  `pembayaran` varchar(50) NOT NULL,
  `nominal` varchar(100) NOT NULL,
  `tanggal` date NOT NULL,
  `staf` varchar(50) DEFAULT NULL,
  `keterangan` text NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data untuk tabel `pembayaran`
--

INSERT INTO `pembayaran` (`id_pembayaran`, `nis`, `nama`, `kelas`, `pembayaran`, `nominal`, `tanggal`, `staf`, `keterangan`) VALUES
(1, '0156374120', 'Siti Sulistini', 'X TKJ 1', 'SPP', '10000000', '2025-11-20', '0981234119', 'Februari, Maret'),
(2, '0156374123', 'Muhammad Arif Maulana', 'X TKJ 1', 'SPP', '14000000', '2025-11-20', '0981234115', 'Maret, April'),
(3, '0156374124', 'Muhammad Nur Aziz', 'X TKJ 1', 'Tabungan', '500000', '2025-11-20', '0981234110', ''),
(7, '0156374123', 'Muhammad Arif Maulana', 'X TKJ 1', 'SPP', '450000', '2025-11-22', '0981234118', 'Februari, Maret, April'),
(8, '0156374120', 'Siti Sulistini', 'X TKJ 1', 'SPP', '400000', '2025-11-27', '0981234115', 'Januari, Februari'),
(9, '0156374120', 'Siti Sulistini', 'X TKJ 1', 'SPP', '100000', '2025-11-27', '0981234119', 'Maret');

-- --------------------------------------------------------

--
-- Struktur dari tabel `pembayaran_lainnya`
--

CREATE TABLE `pembayaran_lainnya` (
  `id_pembayaran_lainnya` int(11) NOT NULL,
  `nominal` varchar(100) NOT NULL,
  `pemasukan` varchar(50) NOT NULL,
  `staf` varchar(50) NOT NULL,
  `tanggal` date NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data untuk tabel `pembayaran_lainnya`
--

INSERT INTO `pembayaran_lainnya` (`id_pembayaran_lainnya`, `nominal`, `pemasukan`, `staf`, `tanggal`) VALUES
(1, '2000000', 'bansos', '', '2025-11-20'),
(2, '5000000', 'pembangunan', '', '2025-11-20'),
(3, '2000000', 'bansos', '', '2025-11-22'),
(4, '100000', 'bansos', '0981234110', '2025-11-27'),
(5, '7000000', 'bansos', '0981234118', '2025-11-27'),
(6, '7000000', 'bansos', '0981234110', '2025-11-27'),
(7, '1000000', 'bansos', '0981234118', '2025-11-28');

-- --------------------------------------------------------

--
-- Struktur dari tabel `pengeluaran`
--

CREATE TABLE `pengeluaran` (
  `id_pengeluaran` int(11) NOT NULL,
  `pengeluaran` varchar(50) NOT NULL,
  `saldo` varchar(100) NOT NULL,
  `nominal` varchar(50) NOT NULL,
  `total` varchar(100) NOT NULL,
  `staf` varchar(20) DEFAULT NULL,
  `tanggal` date NOT NULL,
  `keterangan` varchar(100) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4;

--
-- Dumping data untuk tabel `pengeluaran`
--

INSERT INTO `pengeluaran` (`id_pengeluaran`, `pengeluaran`, `saldo`, `nominal`, `total`, `staf`, `tanggal`, `keterangan`) VALUES
(1, 'SPP', '', '1000000', '', '0981234111', '2025-11-20', 'vdrvrrr'),
(2, 'SPP', '', '1000000', '', '0981234110', '2025-11-20', 'rbwr'),
(3, 'SPP', '', '1000000', '', '0981234112', '2025-11-20', 'db'),
(4, 'Tabungan', '', '100000', '', '0981234112', '2025-11-20', 'crfw'),
(5, 'Tabungan', '', '100000', '', '0981234111', '2025-11-20', 'ddfdrrrrr'),
(6, 'bansos', '', '1000000', '', '0981234111', '2025-11-20', 'gmdtm'),
(7, 'pembangunan', '', '1000000', '', '0981234111', '2025-11-20', 'vsrbarr'),
(8, 'SPP', '', '1000000', '', '0981234111', '2025-11-20', 'mtuk'),
(9, 'SPP', '', '450000', '', '0981234111', '2025-11-22', 'h'),
(10, 'bansos', '', '20000', '', '0981234111', '2025-11-27', '-'),
(11, 'bansos', '', '7000000', '', '0981234111', '2025-11-27', '000'),
(12, 'bansos', '', '7000000', '', '0981234111', '2025-11-27', '--'),
(13, 'bansos', '', '1000000', '', '0981234111', '2025-11-27', '00');

--
-- Indexes for dumped tables
--

--
-- Indeks untuk tabel `data_siswa`
--
ALTER TABLE `data_siswa`
  ADD PRIMARY KEY (`nis`);

--
-- Indeks untuk tabel `data_staff`
--
ALTER TABLE `data_staff`
  ADD PRIMARY KEY (`nip`);

--
-- Indeks untuk tabel `login`
--
ALTER TABLE `login`
  ADD PRIMARY KEY (`username`);

--
-- Indeks untuk tabel `pembayaran`
--
ALTER TABLE `pembayaran`
  ADD PRIMARY KEY (`id_pembayaran`),
  ADD KEY `fk_pembayaran_siswa` (`nis`),
  ADD KEY `fk_pembayaran_staff` (`staf`);

--
-- Indeks untuk tabel `pembayaran_lainnya`
--
ALTER TABLE `pembayaran_lainnya`
  ADD PRIMARY KEY (`id_pembayaran_lainnya`),
  ADD KEY `staf` (`staf`);

--
-- Indeks untuk tabel `pengeluaran`
--
ALTER TABLE `pengeluaran`
  ADD PRIMARY KEY (`id_pengeluaran`),
  ADD KEY `staf` (`staf`);

--
-- AUTO_INCREMENT untuk tabel yang dibuang
--

--
-- AUTO_INCREMENT untuk tabel `pembayaran`
--
ALTER TABLE `pembayaran`
  MODIFY `id_pembayaran` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=11;

--
-- AUTO_INCREMENT untuk tabel `pembayaran_lainnya`
--
ALTER TABLE `pembayaran_lainnya`
  MODIFY `id_pembayaran_lainnya` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT untuk tabel `pengeluaran`
--
ALTER TABLE `pengeluaran`
  MODIFY `id_pengeluaran` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=14;

--
-- Ketidakleluasaan untuk tabel pelimpahan (Dumped Tables)
--

--
-- Ketidakleluasaan untuk tabel `pembayaran`
--
ALTER TABLE `pembayaran`
  ADD CONSTRAINT `fk_pembayaran_siswa` FOREIGN KEY (`nis`) REFERENCES `data_siswa` (`nis`) ON UPDATE CASCADE,
  ADD CONSTRAINT `fk_pembayaran_staff` FOREIGN KEY (`staf`) REFERENCES `data_staff` (`nip`) ON DELETE SET NULL ON UPDATE CASCADE;

--
-- Ketidakleluasaan untuk tabel `pengeluaran`
--
ALTER TABLE `pengeluaran`
  ADD CONSTRAINT `fk_pengeluaran_staf` FOREIGN KEY (`staf`) REFERENCES `data_staff` (`nip`) ON DELETE SET NULL ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
