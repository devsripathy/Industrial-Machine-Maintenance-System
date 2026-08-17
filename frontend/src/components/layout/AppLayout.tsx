import { BarChart3, Bell, ChevronDown, ClipboardList, Factory, FileText, HardHat, LayoutDashboard, LogOut, Package, ShieldCheck, UserCircle2, Users, Wrench } from 'lucide-react';
import type { ReactNode } from 'react';
import { NavLink, useLocation } from 'react-router-dom';
import { useAuth } from '../../context/AuthContext';

const navigation = [
  { label: 'Dashboard', to: '/', icon: LayoutDashboard },
  { label: 'Machines', to: '/machines', icon: Factory },
  { label: 'Work Orders', to: '/work-orders', icon: ClipboardList },
  { label: 'PM Plans', to: '/pm-plans', icon: Wrench },
  { label: 'Inventory', to: '/inventory', icon: Package },
  { label: 'Users & Roles', to: '/users', icon: Users },
  { label: 'Audit Logs', to: '/audit', icon: FileText },
  { label: 'Reports', to: '/reports', icon: BarChart3 },
];

export function AppLayout({ children }: { children: ReactNode }) {
  const { user, logout } = useAuth();
  const location = useLocation();
  const roleName = (user?.roleName ?? '').toLowerCase();
  const isAdmin = roleName.includes('admin') || roleName.includes('manager');

  const visibleNavigation = navigation.filter(({ label }) => {
    if (isAdmin) return true;
    return !['Users & Roles', 'Audit Logs', 'Reports'].includes(label);
  });

  return (
    <div className="min-h-screen bg-slate-950 text-slate-100">
      <div className="flex min-h-screen">
        <aside className="hidden w-72 border-r border-slate-800 bg-slate-900/80 p-5 lg:flex lg:flex-col">
          <div className="mb-8 flex items-center gap-3">
            <div className="flex h-12 w-12 items-center justify-center rounded-xl bg-cyan-500/20 text-cyan-300 ring-1 ring-cyan-500/30">
              <ShieldCheck className="h-6 w-6" />
            </div>
            <div>
              <p className="text-xs uppercase tracking-[0.22em] text-slate-400">SentinelOps</p>
              <h1 className="text-lg font-semibold text-white">CMMS</h1>
            </div>
          </div>

          <nav className="space-y-2">
            {visibleNavigation.map(({ label, to, icon: Icon }) => {
              const isActive = location.pathname === to || (to !== '/' && location.pathname.startsWith(to));
              return (
                <NavLink
                  key={to}
                  to={to}
                  className={`flex items-center gap-3 rounded-xl px-3 py-2.5 text-sm font-medium transition ${
                    isActive ? 'bg-slate-700 text-cyan-300 ring-1 ring-cyan-500/30' : 'text-slate-300 hover:bg-slate-800 hover:text-white'
                  }`}
                >
                  <Icon className="h-4 w-4" />
                  {label}
                </NavLink>
              );
            })}
          </nav>

          <div className="mt-auto rounded-2xl border border-slate-800 bg-slate-950/70 p-4">
            <div className="mb-2 flex items-center gap-2 text-sm text-slate-300">
              <HardHat className="h-4 w-4 text-cyan-300" />
              Maintenance snapshot
            </div>
            <div className="space-y-2 text-sm">
              <div className="flex items-center justify-between text-slate-400">
                <span>Open work orders</span>
                <span className="font-semibold text-white">14</span>
              </div>
              <div className="flex items-center justify-between text-slate-400">
                <span>Critical assets</span>
                <span className="font-semibold text-white">05</span>
              </div>
              <div className="flex items-center justify-between text-slate-400">
                <span>MTTR trend</span>
                <span className="font-semibold text-emerald-300">-8.2%</span>
              </div>
            </div>
          </div>
        </aside>

        <div className="flex-1">
          <header className="border-b border-slate-800 bg-slate-900/80 px-4 py-4 backdrop-blur lg:px-8">
            <div className="flex items-center justify-between gap-4">
              <div className="flex items-center gap-3">
                <div className="flex h-10 w-10 items-center justify-center rounded-lg border border-slate-700 bg-slate-800 text-slate-200 lg:hidden">
                  <Factory className="h-5 w-5" />
                </div>
                <div>
                  <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Operations</p>
                  <h2 className="text-sm font-semibold text-white">SentinelOps Control Center</h2>
                </div>
              </div>

              <div className="flex items-center gap-3">
                <button className="rounded-xl border border-slate-700 bg-slate-800 p-2 text-slate-300 transition hover:border-slate-600 hover:text-white">
                  <Bell className="h-4 w-4" />
                </button>
                <div className="hidden items-center gap-3 rounded-xl border border-slate-700 bg-slate-800 px-3 py-2 md:flex">
                  <div className="flex h-8 w-8 items-center justify-center rounded-full bg-cyan-500/15 text-cyan-300">
                    <UserCircle2 className="h-4 w-4" />
                  </div>
                  <div className="text-left">
                    <p className="text-xs text-slate-400">Signed in</p>
                    <p className="text-sm font-medium text-white">{user?.fullName ?? 'Operator'}</p>
                    <p className="text-[10px] uppercase tracking-[0.14em] text-cyan-300">{user?.roleName ?? 'Operator'}</p>
                  </div>
                  <ChevronDown className="h-4 w-4 text-slate-400" />
                </div>
                <button
                  onClick={logout}
                  className="inline-flex items-center gap-2 rounded-xl border border-rose-500/40 bg-rose-500/10 px-3 py-2 text-sm font-medium text-rose-200 transition hover:bg-rose-500/20"
                >
                  <LogOut className="h-4 w-4" />
                  Logout
                </button>
              </div>
            </div>
          </header>

          <main className="p-4 lg:p-8">{children}</main>
        </div>
      </div>
    </div>
  );
}
