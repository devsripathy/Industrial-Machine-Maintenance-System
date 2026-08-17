import { ShieldCheck, UserCog } from 'lucide-react';
import { useAuth } from '../context/AuthContext';

const users = [
  { name: 'System Administrator', role: 'Administrator', status: 'Active', permissions: 'Full access' },
  { name: 'Maintenance Lead', role: 'Supervisor', status: 'Active', permissions: 'Work order + inventory' },
  { name: 'Shift Technician', role: 'Technician', status: 'Active', permissions: 'Machine status + PM' },
];

export default function UsersPage() {
  const { user } = useAuth();
  const isAdmin = (user?.roleName ?? '').toLowerCase().includes('admin');

  return (
    <div className="space-y-6">
      <div className="flex flex-col gap-3 md:flex-row md:items-end md:justify-between">
        <div>
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Access management</p>
          <h1 className="text-3xl font-semibold text-white">Users & Roles</h1>
        </div>
        <button className="rounded-xl bg-cyan-500 px-4 py-2 text-sm font-semibold text-slate-950 transition hover:bg-cyan-400">
          Invite user
        </button>
      </div>

      <div className="grid gap-4 md:grid-cols-3">
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Active accounts</p>
          <div className="mt-3 text-3xl font-semibold text-white">24</div>
          <p className="mt-1 text-sm text-slate-400">Across 4 role groups</p>
        </div>
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Permission changes</p>
          <div className="mt-3 text-3xl font-semibold text-cyan-300">06</div>
          <p className="mt-1 text-sm text-slate-400">This month</p>
        </div>
        <div className="rounded-2xl border border-slate-800 bg-slate-900 p-5">
          <p className="text-xs uppercase tracking-[0.2em] text-slate-400">Admin visibility</p>
          <div className="mt-3 text-3xl font-semibold text-emerald-300">{isAdmin ? 'ON' : 'Limited'}</div>
          <p className="mt-1 text-sm text-slate-400">Role-based UI access</p>
        </div>
      </div>

      <div className="grid gap-4 lg:grid-cols-3">
        {users.map((userRecord) => (
          <div key={userRecord.name} className="rounded-2xl border border-slate-800 bg-slate-900 p-4">
            <div className="mb-4 flex items-center justify-between">
              <div className="flex items-center gap-2 text-cyan-300">
                <UserCog className="h-4 w-4" />
                <span className="text-xs uppercase tracking-[0.18em]">{userRecord.role}</span>
              </div>
              <div className="rounded-full border border-emerald-500/30 bg-emerald-500/10 px-2 py-1 text-[10px] uppercase tracking-[0.12em] text-emerald-200">
                {userRecord.status}
              </div>
            </div>

            <h2 className="text-lg font-semibold text-white">{userRecord.name}</h2>
            <div className="mt-4 text-sm text-slate-300">
              <div className="flex items-center justify-between">
                <span className="text-slate-400">Permissions</span>
                <span>{userRecord.permissions}</span>
              </div>
            </div>

            <button className="mt-5 inline-flex items-center gap-2 rounded-xl border border-slate-700 bg-slate-950 px-3 py-2 text-sm font-medium text-white hover:border-cyan-500/40">
              <ShieldCheck className="h-4 w-4 text-cyan-300" />
              Manage access
            </button>
          </div>
        ))}
      </div>

      {isAdmin ? (
        <div className="rounded-2xl border border-cyan-500/30 bg-cyan-500/10 p-4 text-sm text-cyan-100">
          Admin controls enabled: full access to user provisioning, role assignment, and audit review is available for this session.
        </div>
      ) : (
        <div className="rounded-2xl border border-amber-500/30 bg-amber-500/10 p-4 text-sm text-amber-100">
          Limited access mode: the current role cannot modify permissions or manage user lifecycle settings.
        </div>
      )}
    </div>
  );
}
