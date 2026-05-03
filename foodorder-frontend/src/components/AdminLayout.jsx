import { Outlet } from 'react-router-dom';
import AdminSidebar from './AdminSidebar';

export default function AdminLayout() {
  return (
    <div className="flex min-h-screen bg-[#fff8f6]">
      <AdminSidebar />
      <main className="flex-1 ml-60 p-8">
        <Outlet />
      </main>
    </div>
  );
}
