import { zodResolver } from '@hookform/resolvers/zod';
import { AlertCircle, Lock, ShieldCheck, UserRound } from 'lucide-react';
import { useState } from 'react';
import { useForm } from 'react-hook-form';
import { Navigate, useNavigate } from 'react-router-dom';
import { z } from 'zod';
import { useAuth } from '../context/AuthContext';

const loginSchema = z.object({
  username: z.string().min(3, 'Username is required.'),
  password: z.string().min(6, 'Password must be at least 6 characters.'),
});

type LoginFormValues = z.infer<typeof loginSchema>;

export default function LoginPage() {
  const { isAuthenticated, login } = useAuth();
  const navigate = useNavigate();
  const [loginError, setLoginError] = useState<string | null>(null);

  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
  } = useForm<LoginFormValues>({
    resolver: zodResolver(loginSchema),
    defaultValues: {
      username: 'admin',
      password: 'AdminPassword123!',
    },
  });

  if (isAuthenticated) {
    return <Navigate to="/" replace />;
  }

  const onSubmit = async (values: LoginFormValues) => {
    try {
      setLoginError(null);
      await login(values.username, values.password);
      navigate('/');
    } catch (error) {
      setLoginError(error instanceof Error ? error.message : 'Unable to sign in.');
    }
  };

  return (
    <div className="flex min-h-screen items-center justify-center bg-[radial-gradient(circle_at_top,_rgba(34,211,238,0.18),_rgba(15,23,42,1)_45%)] px-4 py-16 text-slate-100">
      <div className="grid w-full max-w-6xl overflow-hidden rounded-3xl border border-slate-800 bg-slate-900/80 shadow-2xl shadow-cyan-950/20 backdrop-blur-md lg:grid-cols-2">
        <div className="flex flex-col justify-between bg-slate-950/80 p-8 lg:p-12">
          <div>
            <div className="mb-6 flex h-14 w-14 items-center justify-center rounded-2xl bg-cyan-500/15 text-cyan-300 ring-1 ring-cyan-500/30">
              <ShieldCheck className="h-7 w-7" />
            </div>
            <p className="text-xs uppercase tracking-[0.25em] text-cyan-300">SentinelOps</p>
            <h1 className="mt-4 text-4xl font-bold tracking-tight text-white">Industrial CMMS</h1>
            <p className="mt-4 max-w-md text-base text-slate-300">
              Monitor machine health, schedule maintenance, and keep production uptime high across every critical asset.
            </p>
          </div>

          <div className="mt-8 space-y-4 text-sm text-slate-300">
            <div className="rounded-2xl border border-slate-800 bg-slate-900 p-4">
              <div className="mb-1 font-medium text-white">Operator readiness</div>
              <div className="flex items-center justify-between text-slate-400">
                <span>Critical assets online</span>
                <span className="font-semibold text-emerald-300">94%</span>
              </div>
            </div>
            <div className="rounded-2xl border border-slate-800 bg-slate-900 p-4">
              <div className="mb-1 font-medium text-white">Maintenance backlog</div>
              <div className="flex items-center justify-between text-slate-400">
                <span>Open work orders</span>
                <span className="font-semibold text-cyan-300">14</span>
              </div>
            </div>
          </div>
        </div>

        <div className="p-8 lg:p-12">
          <div className="mb-8">
            <p className="text-sm uppercase tracking-[0.2em] text-slate-400">Secure sign in</p>
            <h2 className="mt-2 text-3xl font-semibold text-white">Welcome back</h2>
          </div>

          <form className="space-y-5" onSubmit={handleSubmit(onSubmit)}>
            <div>
              <label className="mb-2 block text-sm font-medium text-slate-200">Username</label>
              <div className="flex items-center gap-3 rounded-xl border border-slate-700 bg-slate-950 px-3 py-3 focus-within:border-cyan-500">
                <UserRound className="h-4 w-4 text-slate-400" />
                <input
                  {...register('username')}
                  className="w-full bg-transparent text-sm text-white outline-none placeholder:text-slate-500"
                  placeholder="admin"
                />
              </div>
              {errors.username && <p className="mt-2 text-xs text-rose-300">{errors.username.message}</p>}
            </div>

            <div>
              <label className="mb-2 block text-sm font-medium text-slate-200">Password</label>
              <div className="flex items-center gap-3 rounded-xl border border-slate-700 bg-slate-950 px-3 py-3 focus-within:border-cyan-500">
                <Lock className="h-4 w-4 text-slate-400" />
                <input
                  type="password"
                  {...register('password')}
                  className="w-full bg-transparent text-sm text-white outline-none placeholder:text-slate-500"
                  placeholder="••••••••"
                />
              </div>
              {errors.password && <p className="mt-2 text-xs text-rose-300">{errors.password.message}</p>}
            </div>

            {loginError && (
              <div className="flex items-center gap-2 rounded-xl border border-rose-500/40 bg-rose-500/10 px-3 py-2 text-sm text-rose-200">
                <AlertCircle className="h-4 w-4" />
                {loginError}
              </div>
            )}

            <button
              type="submit"
              disabled={isSubmitting}
              className="inline-flex w-full items-center justify-center rounded-xl bg-cyan-500 px-4 py-3 text-sm font-semibold text-slate-950 transition hover:bg-cyan-400 disabled:cursor-not-allowed disabled:opacity-70"
            >
              {isSubmitting ? 'Signing in...' : 'Sign in'}
            </button>
          </form>

          <div className="mt-6 rounded-xl border border-slate-800 bg-slate-950/70 p-4 text-sm text-slate-300">
            Demo credentials
            <div className="mt-2 font-medium text-cyan-300">admin / AdminPassword123!</div>
          </div>
        </div>
      </div>
    </div>
  );
}
